using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Domain.ExModel;

namespace Manager
{
    public class BusinessProductManager: IBusinessProductManager,IDependency
    {

        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public PageDataHelper<object> pdh = new PageDataHelper<object>();
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.Shape==1
                             select new 
                             {
                                 ProductID = a.ID,
                                 Nation = a.Nation,
                                 Url_Asin=a.Url_Asin,
                                 Url_Asin_Value=a.Url_Asin_Value,
                                 ProductImg = a.ProductImg,
                                 Title = a.Title,
                                 Label = a.Label,
                                 OrderType=a.OrderType,
                                 cmtType=a.cmtType,
                                 cmtDay =a.cmtDay,
                                 ProductNumber = a.ProductNumber,
                                 Price = a.Price,
                                 Commission = a.Commission,
                                 TotalMoney = a.TotalMoney,
                                 Status = a.Status       
                             }).ToList();
                pagedata.data=Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize,int status)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.Status == status && a.Shape == 1
                             select new
                             {
                                 ProductID = a.ID,
                                 Nation = a.Nation,
                                 Url_Asin = a.Url_Asin,
                                 Url_Asin_Value = a.Url_Asin_Value,
                                 ProductImg = a.ProductImg,
                                 Title = a.Title,
                                 Label = a.Label,
                                 OrderType = a.OrderType,
                                 cmtType = a.cmtType,
                                 cmtDay = a.cmtDay,
                                 ProductNumber = a.ProductNumber,
                                 Price = a.Price,
                                 Commission = a.Commission,
                                 TotalMoney = a.TotalMoney,
                                 Status = a.Status
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, string title)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.Title.Contains(title) && a.Shape == 1
                             select new
                             {
                                 ProductID = a.ID,
                                 Nation = a.Nation,
                                 Url_Asin = a.Url_Asin,
                                 Url_Asin_Value = a.Url_Asin_Value,
                                 ProductImg = a.ProductImg,
                                 Title = a.Title,
                                 Label = a.Label,
                                 OrderType = a.OrderType,
                                 cmtType = a.cmtType,
                                 cmtDay = a.cmtDay,
                                 ProductNumber = a.ProductNumber,
                                 Price = a.Price,
                                 Commission = a.Commission,
                                 TotalMoney = a.TotalMoney,
                                 Status = a.Status
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, int status,string title)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.Title.Contains(title) && a.Status==status && a.Shape == 1
                             select new
                             {
                                 ProductID = a.ID,
                                 Nation = a.Nation,
                                 Url_Asin = a.Url_Asin,
                                 Url_Asin_Value = a.Url_Asin_Value,
                                 ProductImg = a.ProductImg,
                                 Title = a.Title,
                                 Label = a.Label,
                                 OrderType = a.OrderType,
                                 cmtType = a.cmtType,
                                 cmtDay = a.cmtDay,
                                 ProductNumber = a.ProductNumber,
                                 Price = a.Price,
                                 Commission = a.Commission,
                                 TotalMoney = a.TotalMoney,
                                 Status = a.Status
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 删除产品列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public bool Remove(int ID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                bool flag = false;
                Product bp=new Product();
                bp.ID = ID;
                bp.Shape = 0;
                try
                {
                    //不需要先查询后修改时候使用
                    db.Set<Product>().Attach(bp);
                    db.Entry<Product>(bp).Property("Shape").IsModified = true;
                    //关闭实体有效验证
                    db.Configuration.ValidateOnSaveEnabled = false;
                    if (db.SaveChanges() > 0)
                    {
                        //恢复验证实体有效性（ValidateOnSaveEnabled）这个开关【如果后续有其他操作，记得恢复】
                        db.Configuration.ValidateOnSaveEnabled = true;
                        flag = true;
                        return flag;
                    }
                    else
                    {
                        //恢复验证实体有效性（ValidateOnSaveEnabled）这个开关【如果后续有其他操作，记得恢复】
                        db.Configuration.ValidateOnSaveEnabled = true;
                        return flag;
                    }
                }
                catch (Exception ex)
                {
                    db.Configuration.ValidateOnSaveEnabled = true;
                    return flag;
                }
            }
        }
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>

        public bool AddBusinessProduct(Product businessProduct)
        {
            using (ShopEntities db = new ShopEntities())
            {
                db.Product.Add(businessProduct);
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 编辑产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>
        public bool EditBusinessProduct(Product businessProduct)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var data = db.Product.SingleOrDefault(o => o.ID == businessProduct.ID);
                data.Url_Asin = businessProduct.Url_Asin;
                data.Url_Asin_Value = businessProduct.Url_Asin_Value;
                data.Nation = businessProduct.Nation;
                data.ProductClassID = businessProduct.ProductClassID;
                data.Title = businessProduct.Title;
                data.Label = businessProduct.Label;
                data.ProductDescribe = businessProduct.ProductDescribe;
                data.ProductImg = businessProduct.ProductImg;
                return db.SaveChanges() > 0;
            }
        }
    }
}
