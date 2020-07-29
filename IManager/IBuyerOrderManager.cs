using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface IBuyerOrderManager:IDependency
    {
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="imgs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool AddBuyerOrder(string taskcode, string ordercode,string code, string imgs, string msg,int stepid, dynamic user);
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="imgs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool AddBuyerOrder(string taskcode, string ordercode, string imgs, string msg, int stepid, dynamic user);
        /// <summary>
        /// 重新提交订单
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="imgs"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool AgainBuyerOrder(string OrderCode, string imgs, string message);
        /// <summary>
        /// 查询提交的订单列表
        /// </summary>
        /// <param name="code">订单的唯一标识码</param>
        /// <returns></returns>
        object LoadOrder(string code);
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        ResultPageData<object> GetTaskAll(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code,int type);
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
        ResultPageData<object> GetBusinessTask(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type, int userid);
        /// <summary>
        /// 获取买家任务
        /// </summary>
        /// <returns></returns>
        ResultPageData<object> GetBuyerTask(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type, int userid);
        /// <summary>
        /// 查看任务订单，有参
        /// </summary>
        /// <returns></returns>
        ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize, string code);
        /// <summary>
        /// 查看任务订单，无参
        /// </summary>
        /// <returns></returns>
        ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize);
        /// <summary>
        /// 查看任务订单,商家
        /// </summary>
        /// <returns></returns>
        ResultPageData<object> QueryBusinessTaskOrder(int pageIndex, int pageSize, int userID);
        /// <summary>
        /// 审核订单，驳回时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        bool ExamineOrders(string code, int stepId, int res, string reason, bool usertype,int examine_type);
        /// <summary>
        /// 审核订单,通过时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        bool ExamineOrders(string code, int stepId, int res, bool usertype, int examine_type);
        /// <summary>
        /// 审核任务接口,通过
        /// </summary>
        /// <returns></returns>
        bool ExamineTasks(string code, int res);
        /// <summary>
        /// 审核任务接口,驳回
        /// </summary>
        /// <returns></returns>
        bool ExamineTasks(string code, int res, string reason);

    }
}
