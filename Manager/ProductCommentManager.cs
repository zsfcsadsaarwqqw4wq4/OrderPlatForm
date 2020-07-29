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
    public class ProductCommentManager: IProductCommentManager,IDependency
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 获取商品评论信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryComment(int pageIndex, int pageSize,int pid)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.ProductComment
                             join b in db.BuyerUserInfo on a.UserID equals b.ID 
                             where a.ProductID==pid
                             select new
                             {
                                 CID = a.ID,
                                 Comment=a.Comment,
                                 User = new
                                 {
                                     UserID=b.ID,
                                     UserName=b.UserName,
                                     Email=b.Email,
                                     PhoneNumber=b.PhoneNumber,
                                     Head=b.Head
                                 }

                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool AddComment(int userid, string comment, int pid)
        {
            using (ShopEntities db = new ShopEntities())
            {
                bool flag = false;
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ProductComment pc = new ProductComment();
                        pc.UserID = userid;
                        pc.Comment = comment;
                        pc.ProductID = pid;
                        db.ProductComment.Add(pc);
                        db.SaveChanges();
                        var p = db.Product.SingleOrDefault(o => o.ID == pid);
                        if (p.ProductNumber > 0)
                        {
                            p.CmtNum = p.CmtNum + 1;
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
    }
}
