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
    public class ProductController : BaseController
    {
        public IProductComponent IPC { get; set; }
        public ResultPageData<object> rpd = new ResultPageData<object>();
        int pageIndex;
        int pageSize;
        public const string Product_Sort = "Product_Sort";

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 根据好评查询商品
        /// </summary>
        public JsonResult QueryGoodProduct()
        {
            RequestUser();
            if (resultData.res==500)
            {
                return this.ResultJson(resultData);
            }
            int pageIndex;
            int pageSize;
            if (GetParams("pageIndex") == null && GetParams("pageSize") == null)
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                rpd=IPC.QueryGoodProduct(pageIndex, pageSize);
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
        /// <summary>
        /// 查询热门分类
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryHotClass()
        {
            RequestUser();
            if (resultData.res == 500)
            {
                return this.ResultJson(resultData);
            }
            var data=IPC.QueryHotClass();
            if (data!=null)
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
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 分类查询商品
        /// </summary>
        public JsonResult QueryProduct()
        {
            RequestUser();
            if (resultData.res == 500)
            {
                return this.ResultJson(resultData);
            }
            int productClassID = 0;
            if (GetParams("pageIndex") == null && GetParams("pageSize") == null)
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (GetParams("productClassID") != null)
                {
                    productClassID = int.Parse(GetParams("productClassID"));
                }
                if (productClassID !=0)
                {
                    rpd = IPC.QueryProducts(pageIndex, pageSize, productClassID);
                }
                else
                {
                    rpd = IPC.QueryProduct(pageIndex, pageSize);
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
        /// <summary>
        /// 查询商品接口
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryProducts()
        {
            RequestUser();
            if (resultData.res == 500)
            {
                return this.ResultJson(resultData);
            }
            int productClassID = -1;
            string keyword = string.Empty;
            int order = -1;
            decimal startPrice = -1;
            decimal endPrice = -1;
            if (GetParams("pageIndex") == null && GetParams("pageSize") == null)
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (!string.IsNullOrWhiteSpace(GetParams("productClassID")))
                {
                    productClassID = int.Parse(GetParams("productClassID"));
                }
                if (!string.IsNullOrWhiteSpace(GetParams("keyword")))
                {
                    keyword = GetParams("keyword");
                }
                if (!string.IsNullOrWhiteSpace(GetParams("order")))
                {
                    order = int.Parse(GetParams("order"));
                }
                if (!string.IsNullOrWhiteSpace(GetParams("startPrice")) && !string.IsNullOrWhiteSpace(GetParams("endPrice")))
                {
                    startPrice = decimal.Parse(GetParams("startPrice"));
                    endPrice = decimal.Parse(GetParams("endPrice"));
                }
                if (order==0)
                {
                    rpd = IPC.QueryProductOrderZero(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
                }
                if (order == 1)
                {
                    rpd = IPC.QueryProductOrderOne(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
                }
                if (order == 2)
                {
                    rpd = IPC.QueryProductOrderTwo(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
                }
                if (order == 3)
                {
                    rpd = IPC.QueryProductOrderThree(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
                }
                if (order == 4)
                {
                    rpd = IPC.QueryProductOrderFour(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
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
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryProductDetails()
        {
            RequestUser();
            int pid = 0;
            if (!string.IsNullOrEmpty(GetParams("pid")))
            {
                pid = int.Parse(GetParams("pid"));
            };
            var data= IPC.QueryProductDetail(pid,us.ID);
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
                resultData.msg = "未查到符合条件的数据";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 收藏和点赞
        /// </summary>
        /// <returns></returns>
        public JsonResult CollectionOrGood()
        {
            RequestUser();
            bool isCollected = false;
            bool isGood = false;
            int status1 = 0;
            int status2 = 0;
            if (!string.IsNullOrEmpty(GetParams("isCollected")))
            {
                isCollected = bool.Parse(GetParams("isCollected"));
                status1 = 1;
            }
            if (!string.IsNullOrEmpty(GetParams("isGood")))
            {
                isGood = bool.Parse(GetParams("isGood"));
                status2 = 1;
            }
            int pid = int.Parse(GetParams("pid"));
            if(IPC.CollectionsOrGoods(pid, us.ID, isCollected, isGood, status1, status2))
            {
                resultData.res = 200;
                resultData.msg = "成功";
                return this.ResultJson(resultData);
            }
            else
            {
                resultData.res = 500;
                resultData.msg = "失败";
                return this.ResultJson(resultData);
            }
        }
    }
}