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
    public class ProductCommentComponent : IProductCommentComponent, IDependencys
    {
        public IProductCommentManager IPCM { get; set; }
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        /// <summary>
        /// 获取商品评论信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public ResultPageData<object> QueryComment(int pageIndex, int pageSize, int pid)
        {
            return IPCM.QueryComment(pageIndex, pageSize, pid);
        }
        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool AddComment(int userid, string comment, int pid)
        {
            return IPCM.AddComment(userid, comment, pid);
        }
    }
}
