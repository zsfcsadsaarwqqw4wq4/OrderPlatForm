using Common;
using Domain;
using IComponent;
using Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.Controllers
{
    public class DefaultController : BaseController
    {
        public ClassFicationManager cfm = new ClassFicationManager();
        BuyerUserInfoManager buim=new BuyerUserInfoManager();
        public ResponsePageData<ClassiFication> pagedata = new ResponsePageData<ClassiFication>();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }       
        /// <summary>
        /// 获取登录用户信息接口
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserInfo()
        {
            RequestUser();
            var res = this.us.ID;
            object data = null;
            if (this.us.Level == 2 || this.us.Level == 3)
            {
                data = buim.QueryBuyerUserInfo(this.us.ID);
            }
            if (this.us.Level == 1 || this.us.Level == 5|| this.us.Level==4)
            {
                data = IBUC.QueryBusinessUserInfo(this.us.ID);
            }
            resultData.res = 200;
            resultData.msg = "查询成功";
            resultData.data = data;
            return this.ResultJson(resultData);
        }
        public string Update()
        {
            throw new Exception("这是个我的异常");
        }
    }
}