using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.ExModel;

namespace Manager
{
    public class ProductManager: IProductManager,IDependency
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 根据好评查询商品
        /// </summary>
        public ResultPageData<object> QueryGoodProduct(int pageIndex,int pageSize)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             orderby a.GoodComment descending
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 SalesVolume = a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = db.Set<Product>().Count();
                return pagedata;
            }
        }
        public DataDictionaryManager DDM = new DataDictionaryManager();
        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryProduct(int pageIndex, int pageSize)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.Shape == 1
                             select new
                             {
                                 ID=a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore=a.PriceBefore,
                                 Discount=a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections=a.Collections,
                                 CmtNum=a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProducts(int pageIndex, int pageSize, int productClassID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where a.ProductClassID==productClassID && a.Shape == 1
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber-a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询热门分类
        /// </summary>
        /// <returns></returns>
        public object QueryHotClass()
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             join b in db.ClassiFication on
                             a.ProductClassID equals b.ID
                             orderby a.AddTime descending
                             select new
                             {
                                 ID=(int?)a.ProductClassID,
                                 ClassName=b.ClassName
                             }).ToList();
                return Qurey.Distinct().Take(5);
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProductOrderZero(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product.AsNoTracking()
                             where (productClassID != -1 ? a.ProductClassID == productClassID: true) 
                             && ((!string.IsNullOrEmpty(keyword)) ? a.Title.Contains(keyword): true) 
                             && ((startPrice!=-1 && endPrice!=-1)? a.Price>=startPrice && a.Price<=endPrice: true) 
                             && a.Shape==1
                             || ((productClassID==-1 && string.IsNullOrEmpty(keyword) && startPrice == -1 && endPrice == -1)? true:false) 
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProductOrderOne(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where (productClassID != -1 ? a.ProductClassID == productClassID : true)
                             && ((!string.IsNullOrEmpty(keyword)) ? a.Title.Contains(keyword) : true)
                             && ((startPrice != -1 && endPrice != -1) ? a.Price >= startPrice && a.Price <= endPrice : true)
                             && a.Shape == 1
                             || ((productClassID == -1 && string.IsNullOrEmpty(keyword) && startPrice == -1 && endPrice == -1) ? true : false)
                             orderby a.AddTime descending
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProductOrderTwo(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where (productClassID != -1 ? a.ProductClassID == productClassID : true)
                             && ((!string.IsNullOrEmpty(keyword)) ? a.Title.Contains(keyword) : true)
                             && ((startPrice != -1 && endPrice != -1) ? a.Price >= startPrice && a.Price <= endPrice : true)
                             && a.Shape == 1
                             || ((productClassID == -1 && string.IsNullOrEmpty(keyword) && startPrice == -1 && endPrice == -1) ? true : false)
                             orderby a.Price 
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProductOrderThree(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where (productClassID != -1 ? a.ProductClassID == productClassID : true)
                             && ((!string.IsNullOrEmpty(keyword)) ? a.Title.Contains(keyword) : true)
                             && ((startPrice != -1 && endPrice != -1) ? a.Price >= startPrice && a.Price <= endPrice : true)
                             && a.Shape == 1
                             || ((productClassID == -1 && string.IsNullOrEmpty(keyword) && startPrice == -1 && endPrice == -1) ? true : false)
                             orderby a.Price descending
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProductOrderFour(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.Product
                             where (productClassID != -1 ? a.ProductClassID == productClassID : true)
                             && ((!string.IsNullOrEmpty(keyword)) ? a.Title.Contains(keyword) : true)
                             && ((startPrice != -1 && endPrice != -1) ? a.Price >= startPrice && a.Price <= endPrice : true)
                             && a.Shape == 1
                             || ((productClassID == -1 && string.IsNullOrEmpty(keyword) && startPrice == -1 && endPrice == -1) ? true : false)
                             orderby a.GoodComment descending
                             select new
                             {
                                 ID = a.ID,
                                 RroductImg = a.ProductImg,
                                 Title = a.Title,
                                 Price = a.Price,
                                 PriceBefore = a.PriceBefore,
                                 Discount = a.Discount,
                                 Coupont = a.Coupon,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 GoodComment = a.GoodComment,
                                 Collections = a.Collections,
                                 CmtNum = a.CmtNum
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = Qurey.Count();
                return pagedata;
            }
        }
        /// <summary>
        /// 查询商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public object QueryProductDetail(int pid,int userid)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.Product
                             join d in db.BusinessUserInfo on a.BusinessID equals d.ID
                             join b in db.ClassiFication on a.ProductClassID equals b.ID
                             where a.ID == pid && a.Shape == 1
                             select new
                             {
                                 id = (int?)a.ProductClassID,
                                 ClassName = b.ClassName,
                                 evaluate = (from c in db.User_Product
                                             where c.ProuctID == a.ID && c.UserID == userid
                                             select new
                                             {
                                                 isCollected = c.isCollected ?? false,
                                                 isGood = c.isGood ?? false,
                                                 isReceiveOrder = c.IsReceiveOrder ?? true
                                             }).FirstOrDefault()??new
                                             {
                                                 isCollected =  false,
                                                 isGood =  false,
                                                 isReceiveOrder = true
                                             },
                                 name = a.Title,
                                 des = a.ProductDescribe,
                                 imgs = a.ProductImg,
                                 seller = new
                                 {
                                     id = d.ID,
                                     name = d.UserName,
                                     head = d.Head
                                 },
                                 relations = (from p in db.Product
                                             join f in db.ClassiFication on p.ProductClassID equals f.ID
                                             where f.PID == b.PID && f.ID!=a.ProductClassID
                                             select new
                                             {
                                                 PID = p.ID,
                                                 RroductImg = p.ProductImg,
                                                 Title = p.Title,
                                                 Price = p.Price,
                                                 PriceBefore = p.PriceBefore,
                                                 Discount = p.Discount,
                                                 SalesVolume = p.SalesVolume,
                                                 ProductNumber = p.ProductNumber,
                                                 GoodComment = p.GoodComment,
                                                 Collections = p.Collections,
                                                 CmtNum = p.CmtNum
                                             }).ToList(),
                                 price = a.Price,
                                 priceBefore = a.PriceBefore,
                                 discount = a.Discount,
                                 coupon = a.Coupon,
                                 expired = a.Expired,
                                 commission = a.Commission,
                                 RemnantNumber = a.ProductNumber - a.SalesVolume,
                                 ProductNumber = a.ProductNumber,
                                 Url_Asin = a.Url_Asin,
                                 Url_Asin_Value=a.Url_Asin_Value
                             }).ToList();
                return Query;
            }
        }
        /// <summary>
        /// 修改账户接单权限
        /// </summary>
        /// <returns></returns>
        public object QueryReceivePower(int pid, int userid,bool isReceiveOrder)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var data = db.User_Product.SingleOrDefault(o => o.ProuctID == pid && o.UserID == userid);
                if (data == null)
                {
                    User_Product up = new User_Product();
                    up.ProuctID = pid;
                    up.UserID = userid;
                    up.IsReceiveOrder = isReceiveOrder;
                    db.User_Product.Add(up);
                    return db.SaveChanges() > 0;
                }
                else
                {
                    data.IsReceiveOrder = isReceiveOrder;
                    return db.SaveChanges() > 0;
                }
            }
        }
        /// <summary>
        /// 收藏商品和点赞商品接口
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <param name="isCollected"></param>
        /// <param name="isGood"></param>
        /// <returns></returns>
        public bool CollectionsOrGoods(int pid, int userid, bool isCollected, bool isGood,int status1,int status2)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var data=db.User_Product.SingleOrDefault(o => o.ProuctID == pid && o.UserID == userid);
                if (data !=null)
                {
                    if (status1 == 1)
                    {
                        data.isCollected = isCollected;
                    }
                    if (status2==1)
                    {
                        data.isGood = isGood;
                    }
                    return db.SaveChanges() > 0;
                }
                else
                {
                    User_Product up = new User_Product();
                    up.ProuctID = pid;
                    up.UserID = userid;
                    up.isCollected = isCollected;
                    up.isGood = isGood;
                    db.User_Product.Add(up);
                    return db.SaveChanges() > 0;
                }
            }
        }
        
    }
}
