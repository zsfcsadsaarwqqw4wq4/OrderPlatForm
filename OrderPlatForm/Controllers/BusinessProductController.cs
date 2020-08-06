using Common;
using Domain;
using IComponent;
using Newtonsoft.Json;
using OrderPlatForm.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Domain.ExModel;

namespace OrderPlatForm.Controllers
{
    public class BusinessProductController : BaseController
    {
        public ResultPageData<object> rpd = new ResultPageData<object>();
        public const string test_tatus_index= "test_tatus";
        public const string Url_Asin = "Url_Asin";
        public const string g_country = "g_country";
        public const string comment_type = "comment_type";
        public const string order_type = "Task_type";
        public IDataDictionaryComponent IDDC { get; set; }
        public IBusinessProductComponent IBPC { get; set; }
        public IProductComponent IPC { get; set; }
        // GET: BusinessProduct
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询商家产品
        /// </summary>
        /// <returns></returns>
        public JsonResult QureyBusinessProduct()
        {
            RequestUser();
            int pageIndex;
            int pageSize;
            int test_tatus_value;
            string title = string.Empty;
            DataDictionary result = new DataDictionary();
            if (string.IsNullOrEmpty(GetParams("pageIndex")) && string.IsNullOrEmpty(GetParams("pageSize")))
            {
                resultData.msg = "索引值和页面大小不能为空";
                return this.ResultJson(resultData);
            }
            else
            {
                pageIndex = int.Parse(GetParams("pageIndex"));
                pageSize = int.Parse(GetParams("pageSize"));
                if (GetParams("test_tatus_value")!=null)
                {
                    test_tatus_value=int.Parse(GetParams("test_tatus_value"));
                    result = IDDC.ITEM(test_tatus_index, test_tatus_value);
                }
                if (GetParams("title")!=null)
                {
                    title = GetParams("title");
                }
                if (!string.IsNullOrEmpty(result.Key))
                {
                    if (!string.IsNullOrEmpty(title))
                    {
                        rpd=IBPC.QueryBusinessProduct(pageIndex, pageSize, result.Value, title);
                    }
                    else
                    {
                        rpd=IBPC.QueryBusinessProduct(pageIndex, pageSize, result.Value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(title))
                    {
                        rpd=IBPC.QueryBusinessProduct(pageIndex, pageSize, title);
                    }
                    else
                    {
                        rpd=IBPC.QueryBusinessProduct(pageIndex, pageSize);
                    }
                }
                if (rpd.total != 0)
                {
                    resultData.res = 200;
                    resultData.msg = "查询成功";
                    resultData.data = rpd;
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 200;
                    resultData.msg = "未查到符合条件的数据";
                    return this.ResultJson(resultData);
                }
            }
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <returns></returns>
        public JsonResult RemoveBusinessProduct()
        {
            RequestUser();
            try
            {
                if (resultData.res == 205)
                {
                    resultData.res = 500;
                    resultData.msg = "参数不能为空";
                    return this.ResultJson(resultData);
                }
                else
                {
                    if (GetParams("ID") == null)
                    {
                        resultData.res = 500;
                        resultData.msg = "主键ID不能为空";
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        int ID = int.Parse(GetParams("ID"));
                        if (IBPC.Remove(ID))
                        {
                            resultData.res = 200;
                            resultData.msg = "删除成功";
                            return this.ResultJson(resultData);
                        }
                        else
                        {
                            resultData.res = 500;
                            resultData.msg = "删除失败";
                            return this.ResultJson(resultData);
                        }
                    }
                }
            }
            catch
            {
                resultData.res = 500;
                resultData.msg = "未知异常";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 多个删除
        /// </summary>
        /// <returns></returns>
        public JsonResult RemoveRangeProduct()
        {
            RequestUser();
            try
            {
                if (resultData.res == 205)
                {
                    resultData.res = 500;
                    resultData.msg = "参数不能为空";
                    return this.ResultJson(resultData);
                }
                else
                {
                    if (GetParams("ID") == null)
                    {
                        resultData.res = 500;
                        resultData.msg = "主键ID不能为空";
                        return this.ResultJson(resultData);
                    }
                    else
                    {
                        var data=GetParams("ID");
                        string[] str = GetParams("ID").Replace("[","").Replace("]","").Split(',');
                        List<string> list = new List<string>(str);
                        foreach (var item in list)
                        {
                            IBPC.Remove(int.Parse(item));
                        }
                        resultData.res = 200;
                        resultData.msg = "删除成功";
                        return this.ResultJson(resultData);
                    }
                }
            }
            catch
            {
                resultData.res = 500;
                resultData.msg = "未知异常";
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 添加一个商家产品
        /// </summary>
        /// <returns></returns>
        public JsonResult AddRangeProduct()
        {
            RequestUser();
            string Url_Asin_Value = string.Empty;
            int Url_Asin_Type = 0;
            Product bp = new Product();
            try
            {
                if (!string.IsNullOrEmpty(Request["Url_Asin_Type"]))
                {
                    Url_Asin_Type = int.Parse(Request["Url_Asin_Type"]);
                }
                if (!string.IsNullOrEmpty(Request["Url_Asin_Value"]))
                {
                    Url_Asin_Value = JsonConvert.DeserializeObject<string>(Request["Url_Asin_Value"]);
                }
                int g_country_value = int.Parse(Request["g_country_value"]);

                int ProductClassID = int.Parse(Request["ProductClassID"]);

                string Title = JsonConvert.DeserializeObject<string>(Request["Title"]);
                var listlabel = JsonConvert.DeserializeObject<List<string>>(Request["Label"]);
                string Label = string.Empty;
                foreach (var item in listlabel)
                {
                    Label = Label + "," + item;
                }
                string ProductDescribe = JsonConvert.DeserializeObject<string>(Request["ProductDescribe"]);
                string Img = UploadFile.GetFile();
                string ProductNumber = JsonConvert.DeserializeObject<string>(Request["num"]);
                string Price = JsonConvert.DeserializeObject<string>(Request["price"]);
                string Commission = JsonConvert.DeserializeObject<string>(Request["commission"]);

                int orderType = int.Parse(Request["orderType"]);
                var result1 = IDDC.ITEM(order_type, orderType);
                int cmtType = int.Parse(Request["cmtType"]);
                var result2 = IDDC.ITEM(comment_type, cmtType);                
                string Expired = JsonConvert.DeserializeObject<string>(Request["expired"]);
                string Remark = JsonConvert.DeserializeObject<string>(Request["remark"]);
                string EndTime = JsonConvert.DeserializeObject<string>(Request["endtime"]);
                if (!string.IsNullOrEmpty(Url_Asin_Value))
                {
                    bp.Url_Asin = Url_Asin_Type;
                    bp.Url_Asin_Value = Url_Asin_Value;
                }
                bp.Nation = g_country_value;
                bp.Title = Title;
                bp.OrderType = result1.Value;
                bp.cmtType = result2.Value;
                bp.SalesVolume = 0;
                bp.ProductClassID = ProductClassID;
                bp.EndTime = DateTime.Parse(EndTime);
                bp.AddTime = DateTime.Now;                
                if (!string.IsNullOrWhiteSpace(Label))
                {
                    bp.Label = Label;
                }
                if (!string.IsNullOrWhiteSpace(ProductDescribe))
                {
                    bp.ProductDescribe = ProductDescribe;
                }
                bp.ProductImg = Img;
                bp.ProductNumber = int.Parse(ProductNumber);
                bp.Price = decimal.Parse(Price);
                bp.Commission = decimal.Parse(Commission);
                bp.Shape = 1;
                bp.Status = 1;
                bp.CmtNum = 0;
                bp.BusinessID = this.us.ID;
                if (result2.Value == 2)
                {
                    bp.cmtDay = int.Parse(JsonConvert.DeserializeObject<string>(Request["cmtDay"]));
                }
                bp.Expired = int.Parse(Expired);
                if (Remark!="null")
                {
                    bp.Remark = Remark;
                }
                if (IBPC.AddBusinessProduct(bp))
                {
                    resultData.res = 200;
                    resultData.msg = "添加成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "添加失败";
                    return this.ResultJson(resultData);
                }
            }
            catch(Exception ex)
            {
                resultData.res = 500;
                resultData.msg = ex.Message;
                resultData.data = bp;
                return this.ResultJson(resultData);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public JsonResult EditRangeProduct()
        {
            RequestUser();
            try
            {
                int ProductID = int.Parse(Request["ProductID"]);
                int Url_Asin_Type = int.Parse(Request["Url_Asin_Type"]);
                string Url_Asin_Value = GetParams("Url_Asin_Value");
                int g_country_value = int.Parse(Request["g_country_value"]);
                var results = IDDC.ITEM(g_country, g_country_value);
                int ProductClassID = int.Parse(Request["ProductClassID"]);
                string Title = Request["Title"];
                string Label = Request["Label"];
                string ProductDescribe = Request["ProductDescribe"];
                string Img = UploadFile.GetFile();
                Product bp = new Product();
                bp.Url_Asin = Url_Asin_Type;
                bp.Url_Asin_Value = Url_Asin_Value;
                bp.Nation = g_country_value;
                bp.ID = ProductID;
                bp.Title = Title;
                bp.Label = Label;
                bp.ProductDescribe = ProductDescribe;
                bp.ProductImg = Img;
                if (IBPC.AddBusinessProduct(bp))
                {
                    resultData.res = 200;
                    resultData.msg = "添加成功";
                    return this.ResultJson(resultData);
                }
                else
                {
                    resultData.res = 500;
                    resultData.msg = "添加失败";
                    return this.ResultJson(resultData);
                }
            }
            catch
            {
                resultData.res = 500;
                resultData.msg = "未知异常";
                return this.ResultJson(resultData);
            }
        }        
    }
}