using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class CodeController : Controller
    {
        /// <summary>
        /// 参数
        /// </summary>
        private JObject obj;
        ResponseData resultdata = new ResponseData();
        // GET: Code
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 发送验证码接口
        /// </summary>
        /// <returns></returns>
        public void SendCode()
        {
            string code = string.Empty;
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {
                    throw new Exception("电话号码不能为空");
                }
                obj = JObject.Parse(json);
            }
            string phonenumber = obj["phonenumber"].ToString();
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                code = code + random.Next(0, 9);
            }
            PushMessage.SendMessage(phonenumber, code);
        }
    }
}