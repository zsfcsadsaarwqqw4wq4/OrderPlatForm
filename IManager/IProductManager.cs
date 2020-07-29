using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface IProductManager
    {
        /// <summary>
        /// 根据好评查询商品
        /// </summary>
        ResultPageData<object> QueryGoodProduct(int pageIndex, int pageSize);
        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ResultPageData<object> QueryProduct(int pageIndex, int pageSize);
        /// <summary>
        /// 分类查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProducts(int pageIndex, int pageSize, int productClassID);
        /// <summary>
        /// 查询热门分类
        /// </summary>
        /// <returns></returns>
        object QueryHotClass();
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProductOrderZero(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice);
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProductOrderOne(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice);
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProductOrderTwo(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice);
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProductOrderThree(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice);
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="productClassId">分类ID</param>
        /// <returns></returns>
        ResultPageData<object> QueryProductOrderFour(int pageIndex, int pageSize, int productClassID, string keyword, decimal startPrice, decimal endPrice);
        /// <summary>
        /// 查询商品详情
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        object QueryProductDetail(int pid, int userid);
        /// <summary>
        /// 收藏商品和点赞商品接口
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <param name="isCollected"></param>
        /// <param name="isGood"></param>
        /// <returns></returns>
        bool CollectionsOrGoods(int pid, int userid, bool isCollected, bool isGood, int status1, int status2);
        /// <summary>
        /// 修改账户接单权限
        /// </summary>
        /// <returns></returns>
        object QueryReceivePower(int pid, int userid, bool isReceiveOrder);
    }
}
