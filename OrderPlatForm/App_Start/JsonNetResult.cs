using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OrderPlatForm.App_Start
{
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// 日期格式
        /// </summary>
        public string DateFormatStr { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public JsonNetResult()
        {
        }
        public JsonNetResult(object data)
        {

        }
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = "application/json";
            }
            else
            {
                response.ContentType = this.ContentType;
            }
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting.Indented, DateFormatString = DateFormatStr };
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings());
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}