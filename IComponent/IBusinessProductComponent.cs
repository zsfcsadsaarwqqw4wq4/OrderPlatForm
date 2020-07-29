using Common;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.ExModel;

namespace IComponent
{
    public interface IBusinessProductComponent
    {
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize);
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, int status);
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, string title);
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, int status, string title);
        /// <summary>
        /// 删除产品列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        bool Remove(int ID);
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>

        bool AddBusinessProduct(Product businessProduct);
        /// <summary>
        /// 编辑产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>

        bool EditBusinessProduct(Product businessProduct);
    }
}
