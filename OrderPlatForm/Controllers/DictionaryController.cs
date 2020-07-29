using Common;
using Domain;
using IComponent;
using Manager;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class DictionaryController : BaseController
    {
        public IDataDictionaryComponent IDDC { get; set; }
        ResponseData rd = new ResponseData();
        /// <summary>
        /// 参数
        /// </summary>
        private JObject obj;
        public class ResultUser
        {
            public ResultUser()
            {
                res = 500;
                msg = "请稍后在尝试";
            }
            /// <summary>
            /// 状态码 200 成功 500 失败
            /// </summary>
            public int res { get; set; }
            /// <summary>
            /// 提示信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 返回的数据结果
            /// </summary>
            public object data { get; set; }
        }
        // GET: Dictionary
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 添加数据字典
        /// </summary>
        public JsonResult AddDictionary()
        {
            RequestUser();
            string key= GetParams("Key");
            string Value = GetParams("Value");
            string Color = GetParams("Color");
            string Icon = GetParams("Icon");
            string Custom = GetParams("Custom");
            string Name = GetParams("Name");
            DataDictionaryAddParams ddap = new DataDictionaryAddParams();
            if (!string.IsNullOrEmpty(GetParams("FID")))
            {
                ddap.FID = int.Parse(GetParams("FID"));
            }
            ddap.Key = key;
            ddap.Value = Value;
            ddap.Color = Color;
            ddap.Icon = Icon;
            ddap.Custom = Custom;
            ddap.Name = Name;
            //调用添加数据字典方法
            IDDC.Add(ddap);
            resultData.res = 200;
            resultData.msg = "添加成功";
            return Json(resultData);

        }
        /// <summary>
        /// 删除数据字典
        /// </summary>
        public JsonResult DeleteDictionary()
        {            
            RequestUser();
            int ID;
            int FID;
            if (GetParams("ID") !=null)
            {
                ID = int.Parse(GetParams("ID"));
                //调用添加数据字典方法
                DeleteDataDictionaryParams ddap = new DeleteDataDictionaryParams();
                if (GetParams("FID") != null)
                {
                    FID = int.Parse(GetParams("FID"));
                    ddap.ID = ID;
                    ddap.FID = FID;
                }
                else
                {
                    ddap.ID = ID;
                }
                //调用添加数据字典方法                
                IDDC.Delete(ddap);
                resultData.res = 200;
                resultData.msg = "删除成功";
                return Json(resultData);
            }
            else
            {
                resultData.msg = "ID不能为空";
                return Json(resultData);
            }
        }
        /// <summary>
        /// 编辑字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        public JsonResult EditDictionary()
        {
            RequestUser();
            int fDictionaryID;
            string Color = string.Empty;
            string Custom = string.Empty;
            string Key = string.Empty;
            string Name = string.Empty;
            string Icon = string.Empty;
            string Value=string.Empty;
            int DictionaryID;
            EdtiDataDictionary ddap = new EdtiDataDictionary();
            if (GetParams("fDictionaryID")!=null)
            {
                fDictionaryID = int.Parse(GetParams("fDictionaryID"));
                ddap.fDictionaryID = fDictionaryID;
            }
            if (GetParams("DictionaryID") != null)
            {
                DictionaryID = int.Parse(GetParams("DictionaryID"));
                ddap.DictionaryID = DictionaryID;
            }
            if (GetParams("Color")!=null)
            {
                Color = GetParams("Color");
                ddap.Color = Color;
            }
            if (GetParams("Key") != null)
            {
                Key = GetParams("Key");
                ddap.key = Key;
            }
            if (GetParams("Value") != null)
            {
                Value = GetParams("Value");
                ddap.value = Value;
            }
            if (GetParams("Name") != null)
            {
                Name = GetParams("Name");
                ddap.Name = Name;
            }
            if (GetParams("Icon") != null)
            {
                Icon = GetParams("Icon");
                ddap.icon = Icon;
            }
            if (GetParams("Custom") != null)
            {
                Custom = GetParams("Custom");
                ddap.custom = Custom;
            }
            IDDC.Edit(ddap);
            resultData.res = 200;
            resultData.msg = "编辑成功";
            return Json(resultData);

        }
        /// <summary>
        /// 编辑父级字典
        /// </summary>
        public void FEditDictionary()
        {
            RequestUser();
            int FDictionaryID = int.Parse(GetParams("FDictionaryID"));
            string Name = GetParams("Name");
            string Argument = GetParams("Argument");
            FDataDictionary fd=new FDataDictionary();
            fd.FDictionaryID = FDictionaryID;
            fd.Name = Name;
            fd.Argument = Argument;
            IDDC.Edit(fd);
        }
        /// <summary>
        /// 编辑子级字典
        /// </summary>
        public void ChildEditDictionary()
        {
            RequestUser();
            string Color = GetParams("Color");
            string custom = GetParams("custom");
            string key = GetParams("key");
            string DictionaryID = GetParams("DictionaryID");
            string fDictionaryID = GetParams("fDictionaryID");
            string icon = GetParams("icon");
            string Name = GetParams("Name");
            DataDictionary dd = new DataDictionary();
            dd.Color = Color;
            dd.Custom = custom;
            dd.Key = key;
            dd.DictionaryID = int.Parse(DictionaryID);
            dd.FDictionaryID = int.Parse(fDictionaryID);
            dd.Icon = icon;
            dd.Name = Name;
            IDDC.Edit(dd);
        }

        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public JsonResult QueryDictionary()
        {
            ResultUser resultUser = new ResultUser();
            bool flag = false;
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                {

                }
                else
                {
                    flag = true;
                    obj = JObject.Parse(json);
                }
            }
            if (flag)
            {
                string name = GetParams("name");
                resultUser.res = 200;
                resultUser.msg = "查询成功";
                resultUser.data = IDDC.Query(name);
                return Json(resultUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultUser.res = 200;
                resultUser.msg = "查询成功";
                resultUser.data = IDDC.Query();
                return Json(resultUser, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 根据枚举值获取子级字典
        /// </summary>
        /// <param name="fID">父级ID</param>
        /// <param name="cID">子级ID</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public JsonResult GetDataDictionary()
        {
            RequestUser();
            try
            {
                int fID = int.Parse(GetParams("fID"));
                int value = int.Parse(GetParams("value"));
                DataDictionary dd= IDDC.GetDataDictionary(fID, value);
                if (dd is null)
                {
                    rd.msg = "数据异常";
                    return Json(rd);
                }
                else
                {
                    rd.res = 200;
                    rd.data = dd;
                    return Json(rd);
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetChilderDataDetail()
        {
            RequestUser();
            try
            {
                int id = int.Parse(GetParams("id"));
                DataDictionary dd = IDDC.GetChilderDataDetail(id);
                if (dd is null)
                {
                    rd.msg = "数据异常";
                    return Json(rd);
                }
                else
                {
                    rd.res = 200;
                    rd.data = dd;
                    return Json(rd);
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetChilderDataList()
        {
            RequestUser();
            try
            {
                string ids = GetParams("ids");
                List<DataDictionary> list = IDDC.GetChilderDataList(ids);
                if (list.Count==0)
                {
                    rd.msg = "数据异常";
                    return Json(rd);
                }
                else
                {
                    rd.res = 200;
                    rd.data = list;
                    return Json(rd);
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }
        /// <summary>
        /// 根据父级Argument  获取子级列表
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <returns></returns>
        public JsonResult LIST()
        {
            RequestUser();
            try
            {
                string Argument = GetParams("Argument");
                List<DataDictionary> list = IDDC.LIST(Argument);
                if (list.Count == 0)
                {
                    rd.msg = "数据异常";
                    return Json(rd);
                }
                else
                {
                    rd.res = 200;
                    rd.data = list;
                    return Json(rd);
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }

        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级对象
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        public JsonResult ITEM()
        {
            RequestUser();
            try
            {
                string Argument = GetParams("Argument");
                int value = int.Parse(GetParams("value"));
                string name = GetParams("name");
                DataDictionary dd = null;
                if (name == null)
                {
                    dd = IDDC.ITEM(Argument, value);
                    if (dd == null)
                    {
                        rd.msg = "数据异常";
                        return Json(rd);
                    }
                    else
                    {
                        rd.res = 200;
                        rd.data = dd;
                        return Json(rd);
                    }
                }
                else
                {
                    dd = IDDC.ITEM(Argument, name);
                    if (dd == null)
                    {
                        rd.msg = "数据异常";
                        return Json(rd);
                    }
                    else
                    {
                        rd.res = 200;
                        rd.data = dd;
                        return Json(rd);
                    }
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }
        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        public JsonResult GetCustom()
        {
            RequestUser();
            try
            {
                string Argument = GetParams("Argument");
                int key = int.Parse(GetParams("key"));
                string val = GetParams("val");
                string res = string.Empty;
                if (string.IsNullOrEmpty(val))
                {
                    res = IDDC.GetCustom(Argument, key);
                    if (string.IsNullOrEmpty(res))
                    {
                        rd.msg = "数据异常";
                        return Json(rd);
                    }
                    else
                    {
                        rd.res = 200;
                        rd.data = res;
                        return Json(rd);
                    }
                }
                else
                {
                    res = IDDC.GetCustom(Argument, val);
                    if (string.IsNullOrEmpty(res))
                    {
                        rd.msg = "数据异常";
                        return Json(rd);
                    }
                    else
                    {
                        rd.res = 200;
                        rd.data = res;
                        return Json(rd);
                    }
                }
            }
            catch
            {
                rd.msg = "未知异常";
                return Json(rd);
            }
        }
    }
}