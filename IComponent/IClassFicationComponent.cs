using Common;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IComponent
{
    public interface IClassFicationComponent
    {
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        ResultPageData<object> QueryClass(int pageIndex, int pageSize);
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        ResultPageData<object> QueryClass(int pageIndex, int pageSize, string ClassName);
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        ResultPageData<object> QueryClass(int pageIndex, int pageSize, int ID);
        /// <summary>
        /// 获取全部分类信息接口
        /// </summary>
        List<ClassiFication> QueryClassFication();
        /// <summary>
        /// 添加分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        bool AddClassiFication(ClassiFication cf);
        /// <summary>
        /// 编辑分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        bool EditClassiFication(ClassiFication cf);
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        bool RemoveClassiFication(ClassiFication cf);
        /// <summary>
        /// 根据分类名查询是否有父级  0表示没有查找到数据，-1表示查找到的数据没有父级 ，其他数字表示查找的数据有父级
        /// </summary>
        int QueryClassIsParent(string ClassName);
    }
}
