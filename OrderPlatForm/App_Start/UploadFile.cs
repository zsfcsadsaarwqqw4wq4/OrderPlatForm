using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OrderPlatForm.App_Start
{
    public static class UploadFile
    {
        /// <summary>
        /// 用户上传文件
        /// </summary>
        /// <returns></returns>        
        public static string GetFile()
        {
            string img = string.Empty;
            try
            {
                var file = HttpContext.Current.Request.Files;
                var count = file.Count;
                for (int i=0;i<file.Count;i++)
                {
                    string filetype = file[i].ContentType.Split('/')[1];
                    string fileName = file[i].FileName;
                    var times = DateTime.Now.ToFileTime().ToString();
                    var res = times + '.' + filetype;
                    string filepath = Path.Combine(HttpContext.Current.Server.MapPath(string.Format("~/{0}", "Images")), res);
                    file[i].SaveAs(filepath);
                    img = img+"/Images/" + res + ',';
                }
                return img;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}