using Common;
using Domain;
using IComponent;
using Newtonsoft.Json.Linq;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class BaseController : Controller
    {

        #region 字段
        /// <summary>
        /// 成员信息
        /// </summary>
        public dynamic us;
        /// <summary>
        /// 参数列表
        /// </summary>
        public JObject param;
        /// <summary>
        /// 返回信息实体
        /// </summary>
        public ResponseData resultData = new ResponseData();
        /// <summary>
        /// 属性注入买家
        /// </summary>
        public IBuyerUserInfoComponent IBUIC { get; set; }
        /// <summary>
        /// 属性注入商家
        /// </summary>
        public IBusinessUserInfoComponent IBUC { get; set; }

        #endregion
        [HttpPost]
        public virtual JsonResult RequestUser()
        {
            try
            {
                string token = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(token))
                {
                    throw new TokenException("身份验证失败");
                }
                AuthInfo authInfo = JwtHelper.GetJwtDecode(token);
                //判断token正确性
                if (authInfo == null)
                {
                    throw new TokenException("身份验证失败");
                }
                if (authInfo.Level == 2 || authInfo.Level ==3)
                {
                    this.us = IBUIC.QueryUserNameUser(authInfo.UserName);
                };
                if (authInfo.Level == 1 || authInfo.Level==5 || authInfo.Level==4)
                {
                    this.us = IBUC.QueryUserNameUser(authInfo.UserName);
                }
                //验证身份信息是否正确
                if (us == null || authInfo.UserName != us.UserName)
                {
                    throw new TokenException("身份验证过期,请重新登录");
                };
                DateTime time = DateTime.Now;
                //验证token是否已经过期
                if (time > authInfo.EndTime)
                {
                    throw new TokenException("身份验证过期,请重新登录");
                };
                using (StreamReader stream = new StreamReader(Request.InputStream))
                {
                    string json = stream.ReadToEnd();
                    if (!string.IsNullOrEmpty(json))
                    {
                        try
                        {
                            this.param = JObject.Parse(json);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        resultData.res = 205;
                        return Json(resultData);
                    }
                }
            }
            catch (HttpException ex)
            {
                this.resultData.res = 403;
                this.resultData.msg = ex.Message;
                return Json(resultData);
            }
            resultData.res = 200;
            return Json(resultData);
        }
        /// <summary>
        /// 获取httprequest的值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数值，如果不存在返回空字符串</returns>
        public string GetParams(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            else
            {
                try
                {
                    if (param[key] != null)
                    {
                        return param[key].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 自定义JSON返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="DateFormatStr"></param>
        /// <returns></returns>
        //protected JsonResult ResultJson(object data, string DateFormatStr)
        //{
        //    return new JsonNetResult
        //    {
        //        Data = data,
        //        DateFormatStr = DateFormatStr
        //    };
        //}
        /// <summary>
        /// 自定义JSON返回
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected JsonResult ResultJson(object data)
        {
            return new JsonNetResult
            {
                Data = data
            };
        }
    }
}