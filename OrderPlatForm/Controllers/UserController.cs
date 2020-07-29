using Common;
using Domain;
using IComponent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static Common.EnumHelper;

namespace OrderPlatForm.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IBuyerUserInfoComponent IBUIC { get; set; }
        public IBusinessUserInfoComponent IBUC { get; set; }
        const string BuyerUserLoginInfo= "BuyerUser";
        const string BusinessUserLoginInfo = "BusinessUser";
        /// <summary>
        /// 参数
        /// </summary>
        private JObject obj;
        ResponseData resultdata = new ResponseData();
        /// <summary>
        /// 实例化一个redis帮助类
        /// </summary>
        RedisHelper rh=new RedisHelper();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 买家登录
        /// </summary>
        /// <returns></returns>
        public JsonResult BuyerUserLogin()
        {
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {
                    resultdata.msg = "没有获取到用户名和密码";
                    return Json(resultdata);
                }
                obj = JObject.Parse(json);
            }
            string UserName = obj["username"].ToString();
            string PassWord = obj["password"].ToString();
            Regex r1 = new Regex(@"^[1]+[3,5,6,7,8,9]+\d{9}$");
            Regex r2 = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            BuyerUserInfo bui = null;
            #region 正则验证
            if (r1.IsMatch(UserName))
            {
                bui = IBUIC.QueryPhoneUser(UserName);
            }
            else if (r2.IsMatch(UserName))
            {
                bui = IBUIC.QueryEmailUser(UserName);
            }
            else
            {
                bui = IBUIC.QueryUserNameUser(UserName);
            }
            #endregion
            #region 登录验证
            if (bui == null)
            {
                resultdata.msg = "该账户不存在";
                return Json(resultdata);
            }
            else
            {
                string loginstate = rh.GetString(BuyerUserLoginInfo + bui.ID.ToString());
                if ("1".Equals(loginstate))
                {
                    resultdata.msg = "该账户已经登录，请勿重复登录";
                    return Json(resultdata);
                }
                else
                {
                    if (PassWord.Equals(bui.PassWord))
                    {
                        resultdata.res = 200;
                        resultdata.msg = "登录成功";
                        DateTime StartTime = DateTime.Now;
                        string token = JwtHelper.CreateToken(bui, StartTime);
                        var result = new
                        {
                            token = token,
                            type = bui.Level
                        };
                        resultdata.data = result;

                        //保存用户登录状态
                        DateTime EndTime = StartTime.AddDays(7);
                        TimeSpan Time = EndTime - StartTime;
                        //保存用户登录状态
                        rh.SetString(BuyerUserLoginInfo + bui.ID.ToString(), "1", Time);
                        return Json(resultdata);
                    }
                    else
                    {
                        resultdata.msg = "密码错误";
                        return Json(resultdata);
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 买家注册
        /// </summary>
        /// <returns></returns>
        public JsonResult BuyerRegister()
        {
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {
                    resultdata.msg = "没有获取到用户名和密码";
                    return Json(resultdata);
                }
                obj = JObject.Parse(json);
            }
            string UserName = obj["username"].ToString();
            string PhoneNumber = obj["phonenumber"].ToString();
            string Email = obj["email"].ToString();
            string WechatNumber = obj["wechatnumber"].ToString();
            string PassWord = obj["password"].ToString();
            string EnterpriseName = obj["enterprisename"].ToString();
            string head = obj["head"].ToString();
            string EnterpriseTaxNumber = obj["enterprisetaxnumber"].ToString();
            if (IBUIC.QueryUserNameUser(UserName) != null)
            {
                resultdata.msg = "用户名已存在";
                return Json(resultdata);
            }
            if (IBUIC.QueryPhoneUser(PhoneNumber) != null)
            {
                resultdata.msg = "电话号码已存在 ";
                return Json(resultdata);
            }
            if (IBUIC.QueryEmailUser(Email) != null)
            {
                resultdata.msg = "邮箱已存在 ";
                return Json(resultdata);
            }
            if (string.IsNullOrEmpty(EnterpriseName) && string.IsNullOrEmpty(EnterpriseTaxNumber))
            {
                BuyerUserInfo bui = new BuyerUserInfo();
                bui.UserName = UserName;
                bui.PhoneNumber = PhoneNumber;
                bui.Email = Email;
                bui.WechatNumber = WechatNumber;
                bui.PassWord = PassWord;
                bui.Head = head;
                bui.Money = 0;
                bui.Shape = 0;//此属性表示审核状态
                bui.Level = Convert.ToInt32(PowerEnum.Two);//接单权限
                if (IBUIC.BuyerUserInfoRegister(bui))
                {
                    resultdata.res = 200;
                    resultdata.msg = "注册成功";
                    return Json(resultdata);
                }
                else
                {
                    resultdata.msg = "注册失败";
                    return Json(resultdata);
                }
            }
            else
            {
                BuyerUserInfo bui = new BuyerUserInfo();
                bui.UserName = UserName;
                bui.PhoneNumber = PhoneNumber;
                bui.Email = Email;
                bui.Head = head;
                bui.WechatNumber = WechatNumber;
                bui.PassWord = PassWord;
                bui.EnterpriseName = EnterpriseName;
                bui.EnterpriseTaxNumber = EnterpriseTaxNumber;
                bui.Money = 0;
                bui.Shape = 0;//此属性表示审核状态    
                bui.Level = Convert.ToInt32(PowerEnum.Three);//接单权限 
                if (IBUIC.BuyerUserInfoRegister(bui))
                {
                    resultdata.res = 200;
                    resultdata.msg = "注册成功";
                    return Json(resultdata);
                }
                else
                {
                    resultdata.msg = "注册失败";
                    return Json(resultdata);
                }
            }
        }
        /// <summary>
        /// 商家登录
        /// </summary>
        /// <returns></returns>
        public JsonResult BusinessUserLogin()
        {

            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {
                    resultdata.msg = "没有获取到用户名和密码";
                    return Json(resultdata);
                }
                obj = JObject.Parse(json);
            }
            string UserName = obj["username"].ToString();
            string PassWord = obj["password"].ToString();
            Regex r1 = new Regex(@"^[1]+[3,5,6,7,8,9]+\d{9}$");
            Regex r2 = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            BusinessUserInfo bui = null;
            #region 正则验证
            if (r1.IsMatch(UserName))
            {
                bui = IBUC.QueryPhoneUser(UserName);
            }
            else if (r2.IsMatch(UserName))
            {
                bui = IBUC.QueryEmailUser(UserName);
            }
            else
            {
                bui = IBUC.QueryUserNameUser(UserName);
            }
            #endregion
            #region 登录验证
            if (bui == null)
            {
                resultdata.msg = "该账户不存在";
                return Json(resultdata);
            }
            //else if (bui)
            //{

            //}
            else
            {
                string loginstate = rh.GetString(BusinessUserLoginInfo + bui.ID.ToString());
                if ("1".Equals(loginstate))
                {
                    resultdata.msg = "该账户已经登录，请勿重复登录";
                    return Json(resultdata);
                }
                else
                {
                    if (PassWord.Equals(bui.PassWord))
                    {
                        resultdata.res = 200;
                        resultdata.msg = "登录成功";
                        DateTime StartTime = DateTime.Now;
                        string token = JwtHelper.CreateToken(bui, StartTime);
                        var result = new
                        {
                            token = token,
                            type = bui.Level
                        };
                        resultdata.data = result;
                        DateTime EndTime = StartTime.AddDays(7);
                        TimeSpan Time = EndTime - StartTime;
                        //保存用户登录状态
                        rh.SetString(BusinessUserLoginInfo + bui.ID.ToString(), "1", Time);
                        return Json(resultdata);
                    }
                    else
                    {
                        resultdata.msg = "密码错误";
                        return Json(resultdata);
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 商家注册
        /// </summary>
        /// <returns></returns>
        public JsonResult BusinessRegister()
        {

            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {
                    resultdata.msg = "没有获取到用户名和密码";
                    return Json(resultdata);
                }
                obj = JObject.Parse(json);
            }
            string UserName = obj["username"].ToString();
            string PhoneNumber = obj["phonenumber"].ToString();
            string Email = obj["email"].ToString();
            string WechatNumber = obj["wechatnumber"].ToString();
            string PassWord = obj["password"].ToString();
            string EnterpriseName = obj["enterprisename"].ToString();
            string head = obj["head"].ToString();
            string EnterpriseTaxNumber = obj["enterprisetaxnumber"].ToString();
            if (string.IsNullOrEmpty(EnterpriseName) && string.IsNullOrEmpty(EnterpriseTaxNumber))
            {
                if (IBUC.QueryUserNameUser(UserName) != null)
                {
                    resultdata.msg = "用户名已存在 ";
                    return Json(resultdata);
                }
                if (IBUC.QueryPhoneUser(PhoneNumber) != null)
                {
                    resultdata.msg = "电话号码已存在 ";
                    return Json(resultdata);
                }
                if (IBUC.QueryEmailUser(Email) != null)
                {
                    resultdata.msg = "邮箱已存在 ";
                    return Json(resultdata);
                }
                BusinessUserInfo bui = new BusinessUserInfo();
                bui.UserName = UserName;
                bui.PhoneNumber = PhoneNumber;
                bui.Email = Email;
                bui.Head = head;
                bui.WeChatNumber = WechatNumber;
                bui.PassWord = PassWord;
                bui.EnterpriseName = EnterpriseName;
                bui.EnterpriseTaxNumber = EnterpriseTaxNumber;
                bui.Money = 0;
                bui.Shape = 0;//此属性表示审核状态    
                bui.Level = Convert.ToInt32(PowerEnum.Four);//接单权限   
                if (IBUC.BusinessInfoRegister(bui))
                {
                    resultdata.res = 200;
                    resultdata.msg = "注册成功";
                    return Json(resultdata);
                }
                else
                {
                    resultdata.msg = "注册失败";
                    return Json(resultdata);
                }
            }
            else
            {
                if (IBUC.QueryUserNameUser(UserName) != null)
                {
                    resultdata.msg = "该账户已存在 ";
                    return Json(resultdata);
                }
                if (IBUC.QueryPhoneUser(PhoneNumber) != null)
                {
                    resultdata.msg = "该账户已存在 ";
                    return Json(resultdata);
                }
                if (IBUC.QueryEmailUser(Email) != null)
                {
                    resultdata.msg = "该账户已存在 ";
                    return Json(resultdata);
                }
                BusinessUserInfo bui = new BusinessUserInfo();
                bui.UserName = UserName;
                bui.PhoneNumber = PhoneNumber;
                bui.Email = Email;
                bui.Head = head;
                bui.WeChatNumber = WechatNumber;
                bui.PassWord = PassWord;
                bui.Money = 0;
                bui.Shape = 0;//此属性表示审核状态    
                bui.Level = Convert.ToInt32(PowerEnum.One);//接单权限   
                if (IBUC.BusinessInfoRegister(bui))
                {
                    resultdata.res = 200;
                    resultdata.msg = "注册成功";
                    return Json(resultdata);
                }
                else
                {
                    resultdata.msg = "注册失败";
                    return Json(resultdata);
                }
            }

        }

    }
}