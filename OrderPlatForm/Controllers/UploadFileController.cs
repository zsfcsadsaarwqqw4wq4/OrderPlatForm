using Common;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class UploadFileController : Controller
    {
        ResponseData rd = new ResponseData();
        // GET: ReleaseProduct
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        public JsonResult UploadImages()
        {
            try
            {
                string files=GetFile();
                rd.res = 200;
                rd.msg = "文件上传成功";
                rd.data = files;
                return Json(rd);
            }
            catch(Exception ex)
            {
                rd.res = 500;
                rd.msg = ex.Message;
                return Json(rd);
            }
        }
        /// <summary>
        /// 用户上传文件
        /// </summary>
        /// <returns></returns>        
        public string GetFile()
        {
            string img = string.Empty;
            try
            {          
                var file = Request.Files;
                var count = file.Count;
                for (int i = 0; i < file.Count; i++)
                {
                    string filetype = file[i].ContentType.Split('/')[1];
                    string fileName = file[i].FileName;
                    var times = DateTime.Now.ToFileTime().ToString();
                    var res = times + '.' + filetype;
                    string filepath = Path.Combine(Server.MapPath(string.Format("~/{0}", "Images")), res);
                    file[i].SaveAs(filepath);
                    img = img + "/Images/" + res + ',';
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