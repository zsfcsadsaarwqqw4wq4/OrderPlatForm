using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface IProductCommentManager
    {
        /// <summary>
        /// 获取商品评论信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        ResultPageData<object> QueryComment(int pageIndex, int pageSize, int pid);
        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        bool AddComment(int userid, string comment, int pid);
    }
}
