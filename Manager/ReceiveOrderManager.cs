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
    public class ReceiveOrderManager: IReceiveOrderManager,IDependency
    {
        /// <summary>
        /// 接收订单并生成任务
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool AddOrder(int pid,int num,dynamic user)
        {
            using (ShopEntities db = new ShopEntities())
            {
                bool flag = false;
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ReceiveOrder ro = new ReceiveOrder();
                        ro.Pid = pid;
                        ro.Num = num;
                        ro.UserID = user.ID;
                        ro.Url = "www.xxx.com";
                        ro.Shape = 1;
                        db.ReceiveOrder.Add(ro);
                        db.SaveChanges();
                        var res = db.Product.SingleOrDefault(o => o.ID == pid);
                        Tasks t=new Tasks();
                        t.ProductID = pid;
                        t.BuyerUserID = user.ID;
                        t.Status = 0;//接单任务改为待确认
                        t.Price = res.Price;
                        t.Time = DateTime.Now;
                        t.OrderCode = Guid.NewGuid().ToString("N");
                        db.Tasks.Add(t);
                        db.SaveChanges();
                        var p=db.Product.SingleOrDefault(o=>o.ID==pid);
                        if (p.ProductNumber>0)
                        {
                            p.SalesVolume = p.SalesVolume + num;
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                    return flag;

                }
            }
        }
        /// <summary>
        /// 查询已提交的订单列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public object QueryOrder(int pid,int userid)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.ReceiveOrder.Where(o => o.Pid == pid && o.UserID == userid).ToList();
            }
        }
    }
}
