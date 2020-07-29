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
    public class ClassManagerController : BaseController
    {
        public IClassFicationComponent ICFC { get; set; }
        public ClassiFication cf = new ClassiFication();
        public ResultPageData<object> pagedata= new ResultPageData<object>();
        // GET: ClassManager
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取分类数据接口
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryClassiFication()
        {
            RequestUser();
            int pageIndex;
            int pageSize;
            string ClassName = string.Empty;
            if (GetParams("pageIndex")==null && GetParams("pageSize")==null)
            {
                resultData.msg = "索引值和页面大小不能为空";
                resultData.data = pagedata;
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (GetParams("ClassName")==null)
                {
                    pagedata = ICFC.QueryClass(pageIndex, pageSize);
                    resultData.res = 200;
                    resultData.msg = "查询成功";
                    resultData.data = pagedata;
                    return this.ResultJson(resultData);
                }
                else
                {
                    ClassName = GetParams("ClassName");
                    int res = ICFC.QueryClassIsParent(ClassName);
                    if (res == 0)
                    {
                        resultData.res = 200;
                        resultData.msg = "没找到符合条件的数据";
                        return this.ResultJson(resultData);
                    }
                    if (res == -1)
                    {
                        pagedata = ICFC.QueryClass(pageIndex, pageSize, ClassName);
                        resultData.res = 200;
                        resultData.data = pagedata;
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        pagedata = ICFC.QueryClass(pageIndex, pageSize, res);
                        resultData.res = 200;
                        resultData.data = pagedata;
                        return this.ResultJson(resultData);
                    }
                }
            }            
        }
        /// <summary>
        /// 编辑分类
        /// </summary>
        public JsonResult EditClass()
        {
            RequestUser();
            int ID;
            string ClassName = string.Empty;
            bool Status;
            if (GetParams("ID")!=null)
            {
                ID = int.Parse(GetParams("ID"));
                cf.ID = ID;
                if (GetParams("ClassName") != null)
                {
                    ClassName = GetParams("ClassName");
                    cf.ClassName = ClassName;
                }
                if (GetParams("Status")!=null)
                {
                    Status = bool.Parse(GetParams("Status"));
                    cf.Status = Status;
                }
                if (ICFC.EditClassiFication(cf))
                {
                    resultData.res = 200;
                    resultData.msg = "修改成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "修改失败";
                    return this.ResultJson(resultData);
                }
            }
            else
            {
                resultData.msg = "主键不能为空";
                return this.ResultJson(resultData);
            }            
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        public JsonResult RemoveClass()
        {
            RequestUser();
            try
            {
                int ID = int.Parse(GetParams("ID"));
                cf.ID = ID;
                cf.Shape = 0;
                if (ICFC.RemoveClassiFication(cf))
                {
                    resultData.res = 200;
                    resultData.msg = "删除成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.msg = "删除失败";
                    return this.ResultJson(resultData);
                }
            }
            catch(Exception ex)
            {
                resultData.msg = ex.Message;
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public JsonResult AddFClass()
        {
            RequestUser();
            try
            {
                string ClassName = GetParams("ClassName");
                cf.ClassName = ClassName;
                cf.RecordTime = DateTime.Now;
                cf.PID = 0;
                cf.Shape = 1;
                if (ICFC.AddClassiFication(cf))
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
            catch
            {
                resultData.msg = "分类名不能为空";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 添加子分类
        /// </summary>
        /// <returns></returns>
        public JsonResult AddClass()
        {
            RequestUser();
            try
            {
                int ID = int.Parse(GetParams("ID"));
                string ClassName = GetParams("ClassName");
                cf.PID = ID;
                cf.ClassName = ClassName;
                cf.RecordTime = DateTime.Now;
                cf.Shape = 1;
                if (ICFC.AddClassiFication(cf))
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
            catch
            {
                resultData.msg = "主键和类名不能为空";
                return this.ResultJson(resultData);
            }
        }
        
    }
}