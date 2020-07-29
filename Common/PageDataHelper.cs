using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PageDataHelper<T>:IDisposable where T:class,new()
    {
        // <summary>
        /// 数据库访问上下文对象
        /// </summary>
        private static ShopEntities Context = new ShopEntities();
        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereLamdba">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <returns></returns>
        public List<T> GetPageData(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLamdba, Expression<Func<T, int>> orderLambda)
        {
            var data=Context.Set<T>().AsNoTracking().Where(whereLamdba)
               .OrderBy(orderLambda)
               .Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return data.ToList();
        }
        /// <summary>
        /// 条件查询总数
        /// </summary>
        /// <param name="exprssion"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> exprssion)
        {
            return Context.Set<T>().Count(exprssion);
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
