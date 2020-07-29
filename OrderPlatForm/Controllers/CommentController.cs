using Common;
using IComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class CommentController : BaseController
    {
        public ResultPageData<object> rpd = new ResultPageData<object>();
        public IProductCommentComponent IPCC { get; set; }
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取商品评论
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryProductComment()
        {
            RequestUser();
            try
            {
                if (!string.IsNullOrEmpty(GetParams("pid")) && !string.IsNullOrEmpty(GetParams("pageIndex")) && !string.IsNullOrEmpty(GetParams("pageSize")))
                {
                    int pid = int.Parse(GetParams("pid"));
                    int pageIndex = int.Parse(GetParams("pageIndex"));
                    int pageSize = int.Parse(GetParams("pageSize"));
                    rpd=IPCC.QueryComment(pageIndex, pageSize, pid);
                    if (rpd.total!=0)
                    {
                        resultData.res = 200;
                        resultData.msg = "查询成功";
                        resultData.data = rpd;
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        resultData.res = 200;
                        resultData.msg = "查询成功,但是并没有找到符合条件的数据";
                        resultData.data = rpd;
                        return this.ResultJson(resultData);
                    }
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "商品ID不能为空";
                    return this.ResultJson(resultData);
                }
            }
            catch(Exception ex)
            {
                resultData.res = 500;
                resultData.msg = ex.Message;
                return this.ResultJson(resultData);
            }
        }

    }
}