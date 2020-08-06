using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using OrderPlatForm.App_Start;

namespace OrderPlatForm
{
    /// <summary>
    /// 短信推送类
    /// </summary>
    public class PushMessage : IHttpHandler
    {
        /// <summary>
        /// 签名里面的应用名
        /// </summary>
        private const string SignName = "接单平台";
        /// <summary>
        /// 短信模板code
        /// </summary>
        private const string TemplateCode = "SMS_198675247";

        /// <summary>
        /// 访问api密钥
        /// </summary>
        private const string AccessKeyId = "LTAI4G6jmB1a86FvUMQ8eBZF";
        /// <summary>
        /// 访问api密码
        /// </summary>
        private const string AccessSecret = "a0vNUgf79mONReolZGCIkfi9toNlcX";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 发送短信验证码方法
        /// </summary>
        /// <param name="phonenumber">电话号码</param>
        /// <param name="code">验证码</param>
        public static void SendMessage(string phonenumber,string code)
        {
            /// <summary>
            /// 实例化一个redis帮助类
            /// </summary>
            RedisHelper rh=new RedisHelper();
            // 创建DefaultAcsClient实例并初始化
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", AccessKeyId, AccessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            // 创建API请求并设置参数
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            //必填:待发送手机号
            request.AddQueryParameters("PhoneNumbers", phonenumber);//手机号
            //必填:短信签名-可在短信控制台中找到
            request.AddQueryParameters("SignName", SignName);//签名
            //必填:短信模板-可在短信控制台中找到
            request.AddQueryParameters("TemplateCode", TemplateCode);//模板code
            //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
            request.AddQueryParameters("TemplateParam", "{code:\""+code+"\"}");//模板内容            
            try
            {
                CommonResponse response = client.GetCommonResponse(request);               
                Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                DateTime starttime = DateTime.Now;
                DateTime endtime = DateTime.Now.AddMinutes(5);
                TimeSpan time = endtime - starttime;
                ///保存当前电话号码的验证码信息
                rh.SetString(phonenumber,code, time);
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}