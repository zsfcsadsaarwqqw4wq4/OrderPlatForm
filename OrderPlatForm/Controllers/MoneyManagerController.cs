using Common;
using Domain;
using IComponent;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class MoneyManagerController : BaseController
    {
        public ResponsePageData<MoneyManager> rpd = new ResponsePageData<MoneyManager>();
        public MoneyManager mm=new MoneyManager();
        const string fund_type_Index = "fund_type";
        const string user_type_Index = "user_type";
        const string user_info_Index = "user_info";
        public ICapitalComponent ICC { get; set; }
        public IDataDictionaryComponent IDDC { get; set; }
        // GET: MoneyManager
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询资金记录
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryWithdrawalInfos()
        {
            RequestUser();
            try
            {
                int pageIndex;
                int pageSize;
                int fundType;                
                int userType=0;
                int userInfo;
                DateTime starttimes;
                DateTime endtimes;
                string starttime = string.Empty;
                string endtime = string.Empty;
                string select = string.Empty;
                DataDictionary result1 = new DataDictionary();
                DataDictionary result2 = new DataDictionary();
                DataDictionary result3 = new DataDictionary();
                result3.Key = "";
                if (GetParams("pageIndex") == null && GetParams("pageSize") == null)
                {
                    resultData.msg = "索引值和页面大小不能为空";
                    return this.ResultJson(resultData);
                }
                else
                {
                    pageIndex = int.Parse(GetParams("pageIndex"));
                    pageSize = int.Parse(GetParams("pageSize"));
                    if (GetParams("fundType") !=null)
                    {
                        fundType = int.Parse(GetParams("fundType"));
                        result1 = IDDC.ITEM(fund_type_Index, fundType);
                    }
                    if (GetParams("userType") != null)
                    {
                        userType = int.Parse(GetParams("userType"));
                        result2 = IDDC.ITEM(user_type_Index, userType);
                    }
                    if (GetParams("userInfo") != null && GetParams("select")!=null)
                    {
                        select = GetParams("select");
                        userInfo = int.Parse(GetParams("userInfo"));
                        result3 = IDDC.ITEM(user_info_Index, userInfo);
                    }
                    if (GetParams("starttime") != null && GetParams("endtime") != null)
                    {
                        starttime = GetParams("starttime");
                        endtime = GetParams("endtime");
                    }
                    if (result1.Key.Equals("全部"))
                    {
                        if (result2.Key.Equals("全部"))
                        {
                            if (!string.IsNullOrEmpty(starttime)&& !string.IsNullOrEmpty(endtime))
                            {
                                starttimes = DateTime.Parse(starttime);
                                endtimes = DateTime.Parse(endtime);
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Time >= starttimes && o.Time <= endtimes, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Time >= starttimes && o.Time <= endtimes, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Time >= starttimes && o.Time <= endtimes, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Time >= starttimes && o.Time <= endtimes, o => o.ID);
                                        break;
                                }
                            }
                            else
                            {
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o=>o.ID>0, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select),o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select), o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select), o => o.ID);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
                            {
                                starttimes = DateTime.Parse(starttime);
                                endtimes = DateTime.Parse(endtime);
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode== result2.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value, o => o.ID);
                                        break;
                                }
                            }
                            else
                            {
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Mode == result2.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Mode == result2.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Mode == result2.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Mode == result2.Value, o => o.ID);
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (result2.Key.Equals("全部"))
                        {
                            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
                            {
                                starttimes = DateTime.Parse(starttime);
                                endtimes = DateTime.Parse(endtime);
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Time >= starttimes && o.Time <= endtimes && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.MoneyType== result1.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                }
                            }
                            else
                            {
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
                            {
                                starttimes = DateTime.Parse(starttime);
                                endtimes = DateTime.Parse(endtime);
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Time >= starttimes && o.Time <= endtimes && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                }
                            }
                            else
                            {
                                switch (result3.Key)
                                {
                                    case "":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    //mode字段1:表示充值,2:表示提现
                                    case "用户名":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.UserName.Equals(select) && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "邮箱":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.Email.Equals(select) && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                    case "手机号":
                                        rpd = ICC.QueryMoney(pageIndex, pageSize, o => o.PhoneNumber.Equals(select) && o.Mode == result2.Value && o.MoneyType == result1.Value, o => o.ID);
                                        break;
                                }
                            }
                        }
                    }
                }
                if (rpd.total != 0)
                {
                    resultData.res = 200;
                    resultData.msg = "查询成功";
                    resultData.data = rpd;
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 200;
                    resultData.msg = "未查到符合条件的数据";
                    return this.ResultJson(resultData);
                }
            }
            catch(Exception ex)
            {
                resultData.res = 500;
                resultData.msg = ex.Message;
                return this.ResultJson(resultData);

            }
        }
        /// <summary>
        /// 添加充值记录
        /// </summary>
        /// <returns></returns>
        public JsonResult AddRechargeInfo()
        {
            RequestUser();
            int userInfo;
            int userType;
            int Mode = 0;
            string select = string.Empty;
            DataDictionary result = new DataDictionary();
            select = GetParams("select");
            userInfo = int.Parse(GetParams("userInfo"));
            result = IDDC.ITEM(user_info_Index, userInfo);
            DataDictionary results = new DataDictionary();
            userType = int.Parse(GetParams("userType"));
            results = IDDC.ITEM(user_type_Index, userType);
            decimal Price = decimal.Parse(GetParams("price"));
            if (results.Key == "买家")
            {
                Mode = 2;
                if (result.Key == "用户名")
                {
                    var bui=IBUIC.QueryUserNameUser(select);
                    if (bui!=null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUIC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return Json(resultData);
                    }                    
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUIC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return Json(resultData);
                    }
                }
            }
            if (results.Key == "团队买家")
            {
                Mode = 3;
                if (result.Key == "用户名")
                {
                    var bui = IBUIC.QueryUserNameUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUIC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return Json(resultData);
                    }
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUIC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return Json(resultData);
                    }
                }
            }
            if (results.Key == "卖家")
            {
                Mode = 1;
                if (result.Key == "用户名")
                {
                    var bui = IBUC.QueryUserNameUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return Json(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return Json(resultData);
                    }
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return Json(resultData);
                    }
                }
            }
            mm.Price = Price;
            mm.Mode = Mode;
            mm.MoneyType = 1;
            if (ICC.AddMoney(mm))
            {
                resultData.res = 200;
                resultData.msg = "添加成功";
                return this.ResultJson(resultData);
            }
            else
            {
                resultData.res = 500;
                resultData.msg = "添加失败";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <returns></returns>
        public JsonResult AddWithdrawalInfo()
        {
            RequestUser();
            int userInfo;
            int userType;
            int Mode = 0;
            string select = string.Empty;
            DataDictionary result = new DataDictionary();
            select = GetParams("select");
            userInfo = int.Parse(GetParams("userInfo"));
            result = IDDC.ITEM(user_info_Index, userInfo);
            DataDictionary results = new DataDictionary();
            userType = int.Parse(GetParams("userType"));
            results = IDDC.ITEM(user_type_Index, userType);
            decimal Price = decimal.Parse(GetParams("price"));
            if (results.Key == "买家")
            {
                Mode = 2;
                if (result.Key == "用户名")
                {
                    var bui = IBUIC.QueryUserNameUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUIC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUIC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return this.ResultJson(resultData);
                    }
                }
            }
            if (results.Key == "团队买家")
            {
                Mode = 3;
                if (result.Key == "用户名")
                {
                    var bui = IBUIC.QueryUserNameUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUIC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUIC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return this.ResultJson(resultData);
                    }
                }
            }
            if (results.Key == "卖家")
            {
                Mode = 2;
                if (result.Key == "用户名")
                {
                    var bui = IBUC.QueryUserNameUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "邮箱")
                {
                    var bui = IBUC.QueryEmailUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;

                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的用户不存在";
                        return this.ResultJson(resultData);
                    }
                }
                if (result.Key == "电话号码")
                {
                    var bui = IBUC.QueryPhoneUser(select);
                    if (bui != null)
                    {
                        mm.UserName = bui.UserName;
                        mm.Email = bui.Email;
                        mm.PhoneNumber = bui.PhoneNumber;
                        mm.Time = DateTime.Now;
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "添加的账户不存在";
                        return this.ResultJson(resultData);
                    }
                }
            }
            mm.Price = Price;
            mm.Mode = Mode;
            mm.MoneyType = 2;
            if (ICC.AddMoney(mm))
            {
                resultData.res = 200;
                resultData.msg = "添加成功";
                return this.ResultJson(resultData);
            }
            else
            {
                resultData.msg = "添加失败";
                return this.ResultJson(resultData);
            }
        }
    }
}