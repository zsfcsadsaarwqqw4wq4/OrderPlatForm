using Common;
using Domain;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.ExModel;

namespace Component
{
    public class BusinessProductComponent: IBusinessProductComponent,IDependencys
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pdh = new ResultPageData<object>();
        public IBusinessProductManager IBPM { get; set; }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize)
        {
            return IBPM.QueryBusinessProduct(pageIndex, pageSize);
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, int status)
        {
            return IBPM.QueryBusinessProduct(pageIndex, pageSize, status);
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, string title)
        {
            return IBPM.QueryBusinessProduct(pageIndex, pageSize, title);
        }
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryBusinessProduct(int pageIndex, int pageSize, int status, string title)
        {
            return IBPM.QueryBusinessProduct(pageIndex, pageSize, status, title);
        }
        /// <summary>
        /// 删除产品列表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        public bool Remove(int ID)
        {
            return IBPM.Remove(ID);
        }
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>

        public bool AddBusinessProduct(Product businessProduct)
        {
            return IBPM.AddBusinessProduct(businessProduct);
        }
        /// <summary>
        /// 编辑产品
        /// </summary>
        /// <param name="businessProduct"></param>
        /// <returns></returns>

        public bool EditBusinessProduct(Product businessProduct)
        {
            return IBPM.EditBusinessProduct(businessProduct);
        }
    }
}
