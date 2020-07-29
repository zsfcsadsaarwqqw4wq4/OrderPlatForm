using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IComponent
{
    public interface IProductCommentComponent
    {
        /// <summary>
        /// 获取商品评论
        /// </summary>
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
