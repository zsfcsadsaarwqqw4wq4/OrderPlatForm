using Common;
using Domain;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class ClassFicationComponent : IClassFicationComponent, IDependencys
    {
        public IClassFicationManager ICFM { get; set; }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize)
        {
            return ICFM.QueryClass(pageIndex, pageSize);
        }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize,string ClassName)
        {
            return ICFM.QueryClass(pageIndex, pageSize, ClassName);
        }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize, int ID)
        {
            return ICFM.QueryClass(pageIndex, pageSize, ID);
        }
        /// <summary>
        /// 获取全部分类信息接口
        /// </summary>
        public List<ClassiFication> QueryClassFication()
        {
            return ICFM.QueryClassFication();
        }
        /// <summary>
        /// 添加分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool AddClassiFication(ClassiFication cf)
        {
            return ICFM.AddClassiFication(cf);
        }
        /// <summary>
        /// 编辑分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool EditClassiFication(ClassiFication cf)
        {
            return ICFM.EditClassiFication(cf);
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool RemoveClassiFication(ClassiFication cf)
        {
            return ICFM.RemoveClassiFication(cf);
        }
        /// <summary>
        /// 根据分类名查询是否有父级  0表示没有查找到数据，-1表示查找到的数据没有父级 ，其他数字表示查找的数据有父级
        /// </summary>
        public int QueryClassIsParent(string ClassName)
        {
            return ICFM.QueryClassIsParent(ClassName);
        }
    }
}

