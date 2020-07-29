using Common;
using Domain;
using Manager;
using Newtonsoft.Json;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static Common.EnumHelper;

namespace OrderPlatForm
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoFacConfig.Register();//autofac:控制反转，依赖注入配置 
            new DataDictionaryManager().InitList();
        }
        protected void Application_error()
        {
            //此处处理异常
            HttpContext ctx = HttpContext.Current;
            HttpResponse response = ctx.Response;
            response.ContentType = "application/json;charset=utf-8";
            ResponseData res = new ResponseData
            {
                res = (int)StatusCode.ErrorCode,
                msg = "未知异常,请稍后重试",
                data = string.Empty
            };

            //获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = ctx.Server.GetLastError();
            if (ex is OperateException)
            {
                res.msg = ex.Message;
                response.Write(JsonConvert.SerializeObject(res));
                ctx.Server.ClearError();
                return;
            }
            else if(ex is TokenException)
            {
                res.res = (int)StatusCode.TokenExpired;
                res.msg = ex.Message;
                response.Write(JsonConvert.SerializeObject(res));
                ctx.Server.ClearError();
                return;
            }
            else if (ex is HttpException)
            {
                ctx.Server.ClearError();
                return;
            }
            //实际发生的异常
            Exception iex = ex.InnerException;
            string iexStr = iex == null ? "" : "\r\n\r\nInnerException------" + iex.Message;

            Regex reg = new Regex(@"(WorkPlatform).*(RequestContext\(\))");
            string Message = reg.Match(ex.StackTrace).Value;
            if (string.IsNullOrEmpty(Message))
                Message = ex.Message;
            else
            {
                Message = Message.Replace("WorkPlatform", "").Replace("RequestContext()", "").Replace(".", "/");
                Message += " " + ex.Message;
            }

            res.data = new
            {
                Message,
                ex.StackTrace,
                InnerException = iex == null ? string.Empty : iex.Message
            };
            response.Write(JsonConvert.SerializeObject(res));
            ctx.Server.ClearError();
        }
    }
}
