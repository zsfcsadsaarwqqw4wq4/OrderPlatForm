using Common;
using IComponent;
using Manager;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class TaskController : BaseController
    {

        public IReceiveOrderComponent IROC { get; set; }
        public IBuyerOrderComponent IBOC { get; set; }
        public IProductComponent IPC { get; set; }
        public IProductCommentComponent IPCC { get; set; }
        public ResultPageData<object> rpd = new ResultPageData<object>();
        int pageIndex;
        int pageSize;
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 接收订单
        /// </summary>
        /// <returns></returns>
        public JsonResult ReceiveOrder()
        {
            RequestUser();
            try
            {
                int pid = int.Parse(GetParams("pid"));
                int num = int.Parse(GetParams("num"));
                if (us.Level == 1 || us.Level==4)
                {
                    resultData.msg = "您是商家没有接单权限";
                    return this.ResultJson(resultData);
                }
                if (us.Level == 5)
                {
                    resultData.msg = "您是平台用户没有接单权限";
                    return this.ResultJson(resultData);
                }
                if (us.Level == 2)
                {
                    if (num > 1)
                    {
                        resultData.msg = "您是个人买家接单数量不能大于1";
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        var data = IROC.AddOrder(pid, num, us);                        
                        if (data)
                        {
                            IPC.QueryReceivePower(pid,us.ID,false);
                            var result = IROC.QueryOrder(pid, us.ID);                 
                            resultData.res = 200;
                            resultData.msg = "接单成功";
                            resultData.data = result;
                            return this.ResultJson(resultData);
                        }
                        else
                        {
                            resultData.res = 500;
                            resultData.msg = "接单失败";
                            return this.ResultJson(resultData);
                        }
                    }
                }
                if (us.Level == 3)
                {
                    var data = IROC.AddOrder(pid, num, us);
                    if (data)
                    {
                        IPC.QueryReceivePower(pid, us.ID, false);
                        var result = IROC.QueryOrder(pid, us.ID);
                        resultData.res = 200;
                        resultData.msg = "接单成功";
                        resultData.data = result;
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        resultData.res = 500;
                        resultData.msg = "接单失败";
                        return this.ResultJson(resultData);
                    }
                }
                return this.ResultJson(resultData);
            }
            catch
            {
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 发布评论
        /// </summary>
        /// <returns></returns>
        public JsonResult ReleaseMsg()
        {
            RequestUser();
            try
            {
                int pid = int.Parse(GetParams("pid"));
                string ctn = GetParams("ctn");
                if (IPCC.AddComment(us.ID, ctn, pid))
                {
                    
                    resultData.res = 200;
                    resultData.msg = "发布成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "发布失败";
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
        /// 提交订单接口
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitOrder()
        {
            RequestUser();
            string Img = UploadFile.GetFile();
            string code = string.Empty;
            string msg = string.Empty;
            string ordercode = string.Empty;
            if (!string.IsNullOrEmpty(Request["code"]))
            {
                code = Request["code"];
            }
            string imgs = Img;
            int stepId = int.Parse(Request["stepId"]);
            if (!string.IsNullOrEmpty(Request["msg"]))
            {
                msg = Request["msg"];
            }
            string taskcode = Request["taskcode"];
            if (!string.IsNullOrEmpty(Request["ordercode"]))
            {
                ordercode = Request["ordercode"];
            }
            else
            {
                ordercode = Guid.NewGuid().ToString("N");
            }
            if (!string.IsNullOrEmpty(code))
            {
                if (IBOC.AddBuyerOrder(taskcode, ordercode, code, imgs, msg, stepId, us))
                {
                    resultData.res = 200;
                    resultData.msg = "提交订单成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "提交订单失败";
                    return this.ResultJson(resultData);
                }
            }
            else
            {
                if (IBOC.AddBuyerOrder(taskcode, ordercode,imgs, msg, stepId, us))
                {
                    resultData.res = 200;
                    resultData.msg = "提交订单成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "提交订单失败";
                    return this.ResultJson(resultData);
                }
            }          
        }
        /// <summary>
        /// 重新提交订单接口
        /// </summary>
        /// <returns></returns>
        public JsonResult AgainSubmitOrder()
        {
            RequestUser();
            string Img = UploadFile.GetFile();
            string ordercode = Request["ordercode"];
            string imgs = Img;
            string msg = string.Empty; 
            if (!string.IsNullOrEmpty(Request["msg"]))
            {
                msg = Request["msg"];
            }
            if (IBOC.AgainBuyerOrder(ordercode, imgs, msg))
            {
                resultData.res = 200;
                resultData.msg = "提交订单成功";
                return this.ResultJson(resultData);
            }
            else
            {
                resultData.res = 500;
                resultData.msg = "提交订单失败";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 加载订单列表
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitOrderList()
        {
            RequestUser();
            string code=GetParams("code");//订单号        
            var data=IBOC.LoadOrder(code);
            if (data != null)
            {
                resultData.res = 200;
                resultData.msg = "查询成功";
                resultData.data = data;
                return this.ResultJson(resultData);
            }
            else
            {
                resultData.res = 500;
                resultData.msg = "查询失败";
                resultData.data = data;
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryTasksAll()
        {
            RequestUser();
            int taskStatus = -1;
            string starttime = string.Empty;
            string endtime = string.Empty;
            string keyword = string.Empty;
            string code = string.Empty;
            int type = -1;
            if (string.IsNullOrEmpty(GetParams("pageIndex")) && string.IsNullOrEmpty(GetParams("pageSize")))
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (!string.IsNullOrEmpty(GetParams("taskStatus")))
                {
                    taskStatus = int.Parse(GetParams("taskStatus"));
                }
                if (!string.IsNullOrEmpty(GetParams("stime")) && !string.IsNullOrEmpty(GetParams("etime")))
                {
                    starttime = GetParams("stime");
                    endtime = GetParams("etime");
                }
                if (!string.IsNullOrEmpty(GetParams("keyword")))
                {
                    keyword = GetParams("keyword");
                }
                if (!string.IsNullOrEmpty(GetParams("code")))
                {
                    code = GetParams("code");
                }
                if (!string.IsNullOrEmpty(GetParams("type")))
                {
                    type= int.Parse(GetParams("type"));
                }
                if(us.Level==2 || us.Level == 3)
                {
                    rpd = IBOC.GetBuyerTask(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type, us.ID);
                }
                if(us.Level==5)
                {
                    rpd = IBOC.GetTaskAll(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type);
                }
                if (us.Level == 1 || us.Level==4)
                {
                    rpd = IBOC.GetBusinessTask(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type,us.ID);
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
        }
        /// <summary>
        /// 查看任务订单
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryTasksOrder()
        {
            RequestUser();
            string code = string.Empty;
            if (string.IsNullOrEmpty(GetParams("pageIndex")) && string.IsNullOrEmpty(GetParams("pageSize")))
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (!string.IsNullOrEmpty(GetParams("code")))
                {
                    code = GetParams("code");
                    rpd = IBOC.QueryTaskOrder(pageIndex, pageSize,code);
                }
                else
                {
                    if (us.Level==5)
                    {
                        rpd = IBOC.QueryTaskOrder(pageIndex, pageSize);
                    }
                    if (us.Level == 1 || us.Level == 4)
                    {
                        rpd = IBOC.QueryBusinessTaskOrder(pageIndex, pageSize,us.ID);
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
        }
        /// <summary>
        /// 审核订单接口
        /// </summary>
        public JsonResult ExamineOrder()
        {
            RequestUser();
            //订单号
            string code = string.Empty;
            int stepId = -1;
            int res = -1;
            string reason = string.Empty;
            code = GetParams("code");
            stepId = int.Parse(GetParams("stepId"));
            res = int.Parse(GetParams("res"));
            //审核类型
            int g_examine_type = int.Parse(GetParams("g_examine_type"));
            bool usertype = false;
            if (us.Level==5)
            {
                usertype = true;
            }
            if (res==1)
            {
                reason = GetParams("reason");
                if(IBOC.ExamineOrders(code, stepId, res, reason, usertype, g_examine_type))
                {
                    resultData.res = 200;
                    resultData.msg = "审核成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "审核失败";
                    return this.ResultJson(resultData);
                }
            }
            else
            {
                if (IBOC.ExamineOrders(code, stepId, res, usertype, g_examine_type))
                {
                    resultData.res = 200;
                    resultData.msg = "审核成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "审核失败";
                    return this.ResultJson(resultData);
                }
            }
        }
        /// <summary>
        /// 审核任务接口,平台方账号功能
        /// </summary>
        /// <returns></returns>
        public JsonResult ExaMineTask()
        {
            RequestUser();
            string code = string.Empty;
            int res = -1;
            string reason = string.Empty;
            code = GetParams("code");
            res = int.Parse(GetParams("res"));
            if (res==1)
            {
                reason = GetParams("reason");
                if (IBOC.ExamineTasks(code,res, reason))
                {
                    resultData.res = 200;
                    resultData.msg = "审核成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "审核失败";
                    return this.ResultJson(resultData);
                }
            }
            else
            {
                if (IBOC.ExamineTasks(code, res))
                {
                    resultData.res = 200;
                    resultData.msg = "审核成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "审核失败";
                    return this.ResultJson(resultData);
                }
            }
        }
    }
}