using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class BuyerOrderManager : IBuyerOrderManager, IDependency
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 提交订单,刷单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="imgs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddBuyerOrder(string taskcode,string ordercode,string code,string imgs,string msg, int stepid, dynamic user)
        {
            using (ShopEntities db = new ShopEntities())
            {
                try
                {
                    //根据任务唯一编号查询任务
                    var res=db.Tasks.SingleOrDefault(o=>o.OrderCode.Equals(taskcode));
                    if (res!=null)
                    {
                        var value = db.Product.SingleOrDefault(o => o.ID == res.ProductID);
                        var b=db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(ordercode));
                        if (b != null)
                        {
                            b.Code = code;
                            b.Images = imgs;
                            b.Message = msg;
                            b.order_steps = stepid;
                            b.Time = DateTime.Now;
                            //佣金
                            b.Commission = value.Commission;
                            b.PlatformMessage = null;
                            b.Platform_order_status = null;
                            b.BusinessMessage = null;
                            b.Business_order_status = null;
                            b.OrderType = value.OrderType;
                        }
                        else
                        {
                            BuyerOrder bo = new BuyerOrder();
                            bo.OrderCode = ordercode;
                            bo.Code = code;
                            bo.Images = imgs;
                            bo.Message = msg;
                            bo.order_steps = stepid;
                            bo.Status = 0;
                            bo.UserID = user.ID;
                            bo.TasksID = res.ID;
                            bo.Time = DateTime.Now;
                            bo.Commission = value.Commission;
                            bo.OrderType = value.OrderType;
                            db.BuyerOrder.Add(bo);
                        }
                        return db.SaveChanges() > 0;
                    }
                    //如果任务没有返回false
                    else
                    {
                        return false;
                    }
                    
                }
                catch
                {
                    return false;
                }
            }
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
            using (ShopEntities db = new ShopEntities())
            {
                try
                {
                    var res = db.Tasks.SingleOrDefault(o => o.OrderCode.Equals(taskcode));
                    if (res != null)
                    {
                        var value=db.Product.SingleOrDefault(o => o.ID == res.ProductID);
                        var b = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(ordercode));
                        if (b != null)
                        {                            
                            b.Images = imgs;
                            b.Message = msg;
                            b.order_steps = stepid;
                            b.Time = DateTime.Now;                           
                            b.Commission = value.Commission;
                            b.PlatformMessage = null;
                            b.Platform_order_status = null;
                            b.BusinessMessage = null;
                            b.Business_order_status = null;
                            b.OrderType = value.OrderType;
                        }
                        else
                        {
                            BuyerOrder bo = new BuyerOrder();
                            bo.OrderCode = ordercode;
                            bo.Images = imgs;
                            bo.Message = msg;
                            bo.order_steps = stepid;
                            bo.UserID = user.ID;
                            bo.TasksID = res.ID;
                            bo.Time = DateTime.Now;
                            bo.OrderType = value.OrderType;
                            bo.Commission = value.Commission;
                            if (value.OrderType==0)
                            {
                                bo.Status = 3;    
                            }
                            if (value.OrderType == 1)
                            {
                                bo.Status = 0;
                            }
                            db.BuyerOrder.Add(bo);
                        }
                        return db.SaveChanges() > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
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
            using (ShopEntities db = new ShopEntities())
            {
                try
                {
                    var res = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(OrderCode));
                    if (res != null)
                    {
                        res.Images = imgs;
                        res.Message = message;
                        res.Status = 1;
                        res.Platform_order_status = null;
                        res.PlatformMessage = null;
                        res.Business_order_status = null;
                        res.BusinessMessage = null;
                        return db.SaveChanges() > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 查询提交的订单列表
        /// </summary>
        /// <param name="code">订单的唯一标识码</param>
        /// <returns></returns>
        public object LoadOrder(string code)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.BuyerOrder
                             where a.OrderCode.Equals(code)
                             select new
                             {
                                 info = (from b in db.Tasks
                                         where b.ID == a.TasksID
                                         select new
                                         {
                                             code = a.OrderCode,
                                             price = b.Price,

                                             user = (from c in db.BuyerUserInfo
                                                     where c.ID == b.BuyerUserID
                                                     select new
                                                     {
                                                         id = c.ID,
                                                         name = c.UserName
                                                     })
                                         }),
                                 evidence = new
                                 {
                                     step = a.order_steps,

                                     imgs = new
                                     {
                                         url = a.Images
                                     },
                                     msg = a.Message,
                                     status1 = (int?)a.Platform_order_status,
                                     status2 = (int?)a.Business_order_status,
                                     reason1 = a.PlatformMessage,
                                     reason2 = a.BusinessMessage
                                 }
                             }).FirstOrDefault();
                return Qurey;
            }
        }
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> GetTaskAll(int pageIndex,int pageSize,int taskStatus, string starttime,string endtime,string keyword,string code,int type)
        {
            using (ShopEntities db = new ShopEntities())
            {
                if ((!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime)))
                {
                    DateTime starttimes = DateTime.Parse(starttime);
                    DateTime endtimes = DateTime.Parse(endtime).AddDays(1);
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID
                                 where a.Time >= starttimes && a.Time <= endtimes
                                 && ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)
                                 let c= db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 orderby a.Time descending
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,
                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission=b.Commission,
                                     tasktype =b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay =b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }
                else
                {
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID
                                 where ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)
                                 orderby a.Time descending
                                 let c = db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,
                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission = b.Commission,
                                     tasktype = b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay = b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }

            }
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
            using (ShopEntities db = new ShopEntities())
            {
                if ((!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime)))
                {
                    DateTime starttimes = DateTime.Parse(starttime);
                    DateTime endtimes = DateTime.Parse(endtime).AddDays(1);
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID
                                 where a.Time >= starttimes && a.Time <= endtimes
                                 && ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)
                                 && b.BusinessID == userid
                                 orderby a.Time descending
                                 let c = db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,
                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission = b.Commission,
                                     tasktype = b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay = b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }
                else
                {
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID
                                 where ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)
                                 && b.BusinessID == userid
                                 orderby a.Time descending
                                 let c = db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,

                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission = b.Commission,
                                     tasktype = b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay = b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }

            }
        }
        /// <summary>
        /// 获取买家任务
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> GetBuyerTask(int pageIndex, int pageSize, int taskStatus, string starttime, string endtime, string keyword, string code, int type,int userid)
        {
            using (ShopEntities db = new ShopEntities())
            {
                if ((!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime)))
                {
                    DateTime starttimes=DateTime.Parse(starttime);
                    DateTime endtimes = DateTime.Parse(endtime).AddDays(1);
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID
                                 where a.Time >= starttimes && a.Time <= endtimes
                                 && ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)                                 
                                 && a.BuyerUserID == userid
                                 orderby a.Time descending
                                 let c = db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,
                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission = b.Commission,
                                     tasktype = b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay = b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }
                else
                {
                    var Query = (from a in db.Tasks
                                 join b in db.Product on a.ProductID equals b.ID 
                                 where ((taskStatus != -1) ? a.Status == taskStatus : true)
                                 && ((!string.IsNullOrEmpty(keyword)) ? b.Title.Contains(keyword) : true)
                                 && ((!string.IsNullOrEmpty(code)) ? a.OrderCode.Equals(code) : true)
                                 && ((type != -1) ? b.OrderType == type : true)
                                 && a.BuyerUserID==userid
                                 orderby a.Time descending
                                 let c = db.BuyerOrder.Where(o => o.TasksID == a.ID).ToList()
                                 select new
                                 {
                                     code = a.OrderCode,
                                     name = b.Title,
                                     img = b.ProductImg,
                                     price = b.Price,
                                     priceBefore = b.PriceBefore,
                                     discount = b.Discount,
                                     coupon = b.Coupon,
                                     time = a.Time,
                                     user = (from u in db.BuyerUserInfo
                                             where u.ID == a.BuyerUserID
                                             select new
                                             {
                                                 id = u.ID,
                                                 name = u.UserName
                                             }),
                                     task = new
                                     {
                                         examine = c.Count(s => s.Status == 1),
                                         conduct = c.Count(s => s.Status == 0),
                                         completed = c.Count(s => s.Status == 4),
                                         orderCount = c.Count(),
                                     },
                                     commission = b.Commission,
                                     tasktype = b.OrderType,
                                     ordertype = b.OrderType,
                                     cmtType = b.cmtType,
                                     cmtDay = b.cmtDay,
                                     status = a.Status
                                 }).ToList();
                    pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    pagedata.total = Query.Count();
                    return pagedata;
                }
                
            }
        }
        /// <summary>
        /// 查看任务订单，有参
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize, string code)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.BuyerOrder
                             join b in db.Tasks on a.TasksID equals b.ID
                             join c in db.Product on b.ProductID equals c.ID
                             where b.OrderCode.Equals(code)
                             orderby a.Time descending
                             select new
                             {
                                 code = a.Code,
                                 ordercode=a.OrderCode,
                                 time = a.Time,
                                 ordertype = a.OrderType,
                                 cmtType=c.cmtType,
                                 cmtDay=c.cmtDay,
                                 user = (from c in db.BuyerUserInfo
                                         where c.ID == a.UserID
                                         select new
                                         {
                                             id = c.ID,
                                             name = c.UserName
                                         }).FirstOrDefault(),                                 
                                 status = (int?)a.Status,
                                 product = new
                                 {
                                     title=c.Title,
                                     img=c.ProductImg
                                 },
                                 commission=a.Commission,
                                 step = (int?)a.order_steps
                             }).ToList();
                pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Query.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查看任务订单,无参
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryTaskOrder(int pageIndex, int pageSize)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.BuyerOrder
                             join b in db.Tasks on a.TasksID equals b.ID
                             join x in db.Product on b.ProductID equals x.ID
                             orderby a.Time descending
                             select new
                             {
                                 code = a.Code,
                                 ordercode = a.OrderCode,
                                 time = a.Time,
                                 ordertype=a.OrderType,
                                 cmtType = x.cmtType,                                 
                                 cmtDay = x.cmtDay,
                                 user = (from c in db.BuyerUserInfo
                                         where c.ID == a.UserID
                                         select new
                                         {
                                             id = c.ID,
                                             name = c.UserName
                                         }).FirstOrDefault(),
                                 status = (int?)a.Status,
                                 product = new
                                 {
                                     title = x.Title,
                                     img = x.ProductImg
                                 },
                                 commission = a.Commission,
                                 step = (int?)a.order_steps
                             }).ToList();
                pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Query.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查看任务订单,商家
        /// </summary>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessTaskOrder(int pageIndex, int pageSize,int userID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.BuyerOrder
                             join b in db.Tasks on a.TasksID equals b.ID
                             join x in db.Product on b.ProductID equals x.ID
                             where x.BusinessID == userID
                             orderby a.Time descending
                             select new
                             {
                                 code = a.Code,
                                 ordercode = a.OrderCode,
                                 time = a.Time,
                                 ordertype = a.OrderType,
                                 cmtType = x.cmtType,
                                 cmtDay = x.cmtDay,
                                 user = (from c in db.BuyerUserInfo
                                         where c.ID == a.UserID
                                         select new
                                         {
                                             id = c.ID,
                                             name = c.UserName
                                         }).FirstOrDefault(),
                                 status = (int?)a.Status,
                                 product = new
                                 {
                                     title = x.Title,
                                     img = x.ProductImg
                                 },
                                 commission = a.Commission,
                                 step = (int?)a.order_steps
                             }).ToList();
                pagedata.data = Query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Query.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 审核订单，驳回时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        public bool ExamineOrders(string code,int stepId,int res,string reason, bool usertype,int examine_type)
        {
            using (ShopEntities db = new ShopEntities())
            {
                if (usertype)
                {
                    var data = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(code) && o.order_steps == stepId);
                    if (data != null)
                    {
                        if (examine_type==0)
                        {
                            data.Platform_order_status = res;
                            data.PlatformMessage = reason;
                        }
                        if (examine_type==1)
                        {
                            data.Business_order_status = res;
                            data.BusinessMessage = reason;
                        }
                        data.Status = 2;
                        return db.SaveChanges() > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var data = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(code) && o.order_steps == stepId);
                    if (data != null)
                    {
                        data.Business_order_status = res;
                        data.BusinessMessage = reason;
                        data.Status = 2;
                        return db.SaveChanges() > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 审核订单,通过时候调用
        /// </summary>
        /// <param name="code">唯一订单编号</param>
        /// <param name="stepId">步骤ID</param>
        /// <param name="res">审核结果</param>
        /// <param name="reason">驳回理由</param>
        /// <returns></returns>
        public bool ExamineOrders(string code, int stepId, int res,bool usertype,int examine_type)
        {
            using (ShopEntities db = new ShopEntities())
            {
                ///当前账号是平台账号
                if (usertype)
                {
                    var data = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(code) && o.order_steps == stepId);
                    if (data != null)
                    {
                        var task = db.Tasks.SingleOrDefault(o => o.ID == data.TasksID);
                        var p = db.Product.SingleOrDefault(o => o.ID == task.ProductID);
                        //该平台方账号以平台方身份审核
                        if (examine_type == 0)
                        {
                            data.Platform_order_status = res;    
                            //判断双方是否都审核通过
                            if (data.Business_order_status == 0)
                            {                              
                                //表示直评
                                if (data.OrderType == 1)
                                {
                                    // 表示当前订单的步骤直评确认
                                    if (stepId == 0)
                                    {
                                        //判断当前任务是否需要保留评论,如果评论保留天数则当前订单没有全部完成
                                        if (p.cmtType==2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.8) ;
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.8);
                                            db.SaveChanges();
                                        }else
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission;
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission;
                                            db.SaveChanges();
                                        }
                                    }
                                    //当前步骤是评论考察
                                    if (stepId==1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                    }
                                }
                                //表示刷单
                                else
                                {
                                    //表示当前订单的步骤为发货确认
                                    if (stepId == 3)
                                    {
                                        //只购买不留评
                                        if (p.cmtType == 1)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                        }
                                        //评论一次或评论保留x天
                                        else
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤为到货确认
                                    if (stepId == 2)
                                    {
                                        //只购买不留评
                                        if (p.cmtType == 1)
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                        }
                                        //购买需要评论一次
                                        if(p.cmtType==0)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                        //购买需要留评x天
                                        if (p.cmtType == 2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤直评确认
                                    if (stepId == 0)
                                    {
                                        //只评论一次
                                        if (p.cmtType == 1)
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                            db.SaveChanges();
                                        }
                                        //评论保留x天
                                        if (p.cmtType == 2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤直评确认
                                    if (stepId == 1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        //该平台方账号以商家身份审核
                        if (examine_type == 1)
                        {
                            data.Business_order_status = res;
                            if (data.Platform_order_status == 0)
                            {
                                //表示直评
                                if (data.OrderType == 1)
                                {
                                    // 表示当前订单的步骤直评确认
                                    if (stepId == 0)
                                    {
                                        //判断当前任务是否需要保留评论,如果评论保留天数则当前订单没有全部完成
                                        if (p.cmtType == 2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.8);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.8);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission;
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission;
                                            db.SaveChanges();
                                        }
                                    }
                                    //当前步骤是评论考察
                                    if (stepId == 1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                    }
                                }
                                //表示刷单
                                else
                                {
                                    //表示当前订单的步骤为发货确认
                                    if (stepId == 3)
                                    {
                                        //只购买不留评
                                        if (p.cmtType == 1)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                        }
                                        //评论一次或评论保留x天
                                        else
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤为到货确认
                                    if (stepId == 2)
                                    {
                                        //只购买不留评
                                        if (p.cmtType == 1)
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                            db.SaveChanges();
                                        }
                                        //购买需要评论一次
                                        if (p.cmtType == 0)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                        //购买需要留评x天
                                        if (p.cmtType == 2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤直评确认
                                    if (stepId == 0)
                                    {
                                        //只评论一次
                                        if (p.cmtType == 1)
                                        {
                                            data.Status = 4;
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                            db.SaveChanges();
                                        }
                                        //评论保留x天
                                        if (p.cmtType == 2)
                                        {
                                            var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                            buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                            db.SaveChanges();
                                            var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                            busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                            db.SaveChanges();
                                        }
                                    }
                                    //表示当前订单的步骤直评确认
                                    if (stepId == 1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                ///表示当前账号是商家
                else
                {
                    var data = db.BuyerOrder.SingleOrDefault(o => o.OrderCode.Equals(code) && o.order_steps == stepId);
                    if (data != null)
                    {
                        var task = db.Tasks.SingleOrDefault(o => o.ID == data.TasksID);
                        var p = db.Product.SingleOrDefault(o => o.ID == task.ProductID);
                        //判断平台方是否审核通过
                        if (data.Platform_order_status == 0)
                        {
                            data.Business_order_status = res;
                            //表示直评
                            if (data.OrderType == 1)
                            {
                                // 表示当前订单的步骤直评确认
                                if (stepId == 0)
                                {
                                    //判断当前任务是否需要保留评论,如果评论保留天数则当前订单没有全部完成
                                    if (p.cmtType == 2)
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.8);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.8);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission;
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission;
                                        db.SaveChanges();
                                    }
                                }
                                //当前步骤是评论考察
                                if (stepId == 1)
                                {
                                    data.Status = 4;
                                    var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                    buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                    db.SaveChanges();
                                    var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                    busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                    db.SaveChanges();
                                }
                            }
                            //表示刷单
                            else
                            {
                                //表示当前订单的步骤为发货确认
                                if (stepId == 3)
                                {
                                    //只购买不留评
                                    if (p.cmtType == 1)
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                        db.SaveChanges();
                                    }
                                    //评论一次或评论保留x天
                                    else
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                    }
                                }
                                //表示当前订单的步骤为到货确认
                                if (stepId == 2)
                                {
                                    //只购买不留评
                                    if (p.cmtType == 1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.5);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.5);
                                        db.SaveChanges();
                                    }
                                    //购买需要评论一次
                                    if (p.cmtType == 0)
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                    }
                                    //购买需要留评x天
                                    if (p.cmtType == 2)
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.4);
                                        db.SaveChanges();
                                    }
                                }
                                //表示当前订单的步骤直评确认
                                if (stepId == 0)
                                {
                                    //只评论一次
                                    if (p.cmtType == 1)
                                    {
                                        data.Status = 4;
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.2);
                                        db.SaveChanges();
                                    }
                                    //评论保留x天
                                    if (p.cmtType == 2)
                                    {
                                        var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                        buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                        var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                        busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                        db.SaveChanges();
                                    }
                                }
                                //表示当前订单的步骤直评确认
                                if (stepId == 1)
                                {
                                    data.Status = 4;
                                    var buser = db.BusinessUserInfo.SingleOrDefault(o => o.ID == p.BusinessID);
                                    buser.Money = buser.Money - data.Commission * Convert.ToDecimal(0.1);
                                    db.SaveChanges();
                                    var busers = db.BuyerUserInfo.SingleOrDefault(o => o.ID == task.BuyerUserID);
                                    busers.Money = busers.Money + data.Commission * Convert.ToDecimal(0.1);
                                    db.SaveChanges();
                                }
                            }
                        }
                        //表示当前步骤平台方审核并未通过
                        else
                        {
                            data.Business_order_status = res;
                        }
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 审核任务接口,通过
        /// </summary>
        /// <returns></returns>
        public bool ExamineTasks(string code,int res)
        {
            using (ShopEntities db=new ShopEntities())
            {
                var data = db.Tasks.SingleOrDefault(o=>o.OrderCode.Equals(code));
                if (data != null)
                {
                    data.Platform_order_status = res;
                    data.Status = 1;                    
                    return db.SaveChanges() > 0;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 审核任务接口,驳回
        /// </summary>
        /// <returns></returns>
        public bool ExamineTasks(string code, int res, string reason)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var data = db.Tasks.SingleOrDefault(o => o.OrderCode.Equals(code));
                if (data != null)
                {
                    data.Platform_order_status = res;
                    data.PlatformMessage = reason;
                    data.Status = 4;
                    return db.SaveChanges() > 0;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
