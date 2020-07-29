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
    public class ProductComponent : IProductComponent, IDependencys
    {
        public IProductManager IPM { get; set; }
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 分页泛型类
        /// </summary>
        public PageDataHelper<object> pdh = new PageDataHelper<object>();
        /// <summary>
        /// 根据好评查询商品
        /// </summary>
        public ResultPageData<object> QueryGoodProduct(int pageIndex, int pageSize)
        {
            return IPM.QueryGoodProduct(pageIndex, pageSize);
        }
        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryProduct(int pageIndex, int pageSize)
        {
            return IPM.QueryProduct(pageIndex, pageSize);
        }
        /// <summary>
        /// 分类查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        public ResultPageData<object> QueryProducts(int pageIndex, int pageSize, int productClassID)
        {
            return IPM.QueryProducts(pageIndex, pageSize, productClassID);
        }
        /// <summary>
        /// 查询热门分类
        /// </summary>
        /// <returns></returns>
        public object QueryHotClass()
        {
            return IPM.QueryHotClass();
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
            return IPM.QueryProductOrderZero(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
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
            return IPM.QueryProductOrderOne(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
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
            return IPM.QueryProductOrderTwo(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
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
            return IPM.QueryProductOrderThree(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
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
            return IPM.QueryProductOrderFour(pageIndex, pageSize, productClassID, keyword, startPrice, endPrice);
        }
        /// <summary>
        /// 查询商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public object QueryProductDetail(int pid, int userid)
        {
            return IPM.QueryProductDetail(pid, userid);
        }
        /// <summary>
        /// 收藏商品和点赞商品接口
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <param name="isCollected"></param>
        /// <param name="isGood"></param>
        /// <returns></returns>
        public bool CollectionsOrGoods(int pid, int userid, bool isCollected, bool isGood, int status1, int status2)
        {
            return IPM.CollectionsOrGoods(pid, userid, isCollected, isGood,  status1, status2);
        }
        /// <summary>
        /// 修改账户接单权限
        /// </summary>
        /// <returns></returns>
        public object QueryReceivePower(int pid, int userid, bool isReceiveOrder)
        {
            return IPM.QueryReceivePower(pid, userid, isReceiveOrder);
        }
    }
}
