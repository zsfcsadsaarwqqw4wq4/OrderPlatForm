using Common;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class BuyerOrderComponent: IBuyerOrderComponent,IDependencys
    { 
        public IBuyerOrderManager IBOM { get; set; }
        /// <summary>
        /// 提交订单,刷单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="imgs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddBuyerOrder(string taskcode, string ordercode,string code, string imgs, string msg,int stepid, dynamic user)
        {
            return IBOM.AddBuyerOrder(taskcode, ordercode, code, imgs, msg, stepid, user);
        }
        /// <summary>
        /// 提交订单,直评
        /// </summary>
        /// <param name="code"></param>
        /// <param name="imgs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddBuyerOrder(string taskcode, string ordercode, string imgs, string msg, int stepid, dynamic user)
        {
            return IBOM.AddBuyerOrder(taskcode, ordercode, imgs, msg, stepid, user);
        }
        /// <summary>
        /// 重新提交订单
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="imgs"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AgainBuyerOrder(string OrderCode, string imgs, string message)
        {
            return IBOM.AgainBuyerOrder(OrderCode, imgs, message);
        }
        /// <summary>
        /// 查询提交的订单列表
        /// </summary>
        /// <param name="code">订单的唯一标识码</param>
        /// <returns></returns>
        public object LoadOrder(string code)
        {
            return IBOM.LoadOrder(code);
        }
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> GetTaskAll(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type)
        {
            return IBOM.GetTaskAll(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type);
        }
        /// <summary>
        /// 获取买家任务
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> GetBuyerTask(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type, int userid)
        {
            return IBOM.GetBuyerTask(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type, userid);
        }
        /// <summary>
        /// 获取商家任务
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskStatus"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="keyword"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResultPageData<object> GetBusinessTask(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type, int userid)
        {
            return IBOM.GetBusinessTask(pageIndex, pageSize, taskStatus, starttime, endtime, keyword, code, type, userid);
        }
        /// 查看任务订单，有参
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize, string code)
        {
            return IBOM.QueryTaskOrder(pageIndex, pageSize,code);
        }
        /// <summary>
        /// 查看任务订单，无参
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize)
        {
            return IBOM.QueryTaskOrder(pageIndex, pageSize);
        }
        /// <summary>
        /// 查看任务订单,商家
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessTaskOrder(int pageIndex, int pageSize, int userID)
        {
            return IBOM.QueryBusinessTaskOrder(pageIndex, pageSize, userID);
        }
        /// <summary>
        /// 审核订单，驳回时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        public bool ExamineOrders(string code, int stepId, int res, string reason, bool usertype, int examine_type)
        {
            return IBOM.ExamineOrders(code, stepId, res, reason, usertype, examine_type);
        }
        /// <summary>
        /// 审核订单,通过时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        public bool ExamineOrders(string code, int stepId, int res, bool usertype, int examine_type)
        {
            return IBOM.ExamineOrders(code, stepId, res, usertype, examine_type);
        }
        /// <summary>
        /// 审核任务接口,通过
        /// </summary>
        /// <returns></returns>
        public bool ExamineTasks(string code, int res)
        {
            return IBOM.ExamineTasks(code, res);
        }
        /// <summary>
        /// 审核任务接口,驳回
        /// </summary>
        /// <returns></returns>
        public bool ExamineTasks(string code, int res, string reason)
        {
            return IBOM.ExamineTasks(code, res, reason);
        }
    }
}
