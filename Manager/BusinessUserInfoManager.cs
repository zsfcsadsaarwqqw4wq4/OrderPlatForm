using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class BusinessUserInfoManager : IBusinessUserInfoManager, IDependency
    {
        /// <summary>
        /// 添加一个商家用户
        /// </summary>
        public bool BusinessInfoRegister(BusinessUserInfo bui)
        {
            using (ShopEntities db = new ShopEntities())
            {
                db.BusinessUserInfo.Add(bui);
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 获取商家账户信息
        /// </summary>
        /// <returns></returns>
        public BusinessUserInfo GetBUInfo(BusinessUserInfo bui)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BusinessUserInfo.SingleOrDefault(o => o.UserName.Equals(bui.UserName) && o.PassWord.Equals(bui.PassWord));
            }
        }
        /// <summary>
        /// 根据用户名搜索用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryUserNameUser(string UserName)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BusinessUserInfo.SingleOrDefault(o => o.UserName.Equals(UserName));
            }
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryPhoneUser(string PhoneNumber)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BusinessUserInfo.SingleOrDefault(o => o.PhoneNumber.Equals(PhoneNumber));
            }
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryEmailUser(string email)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BusinessUserInfo.SingleOrDefault(o => o.Email.Equals(email));
            }
        }
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        public object QueryBusinessUserInfo(int ID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.BusinessUserInfo
                             join b in db.Business_Product on a.ID equals b.BusinessUserInfoID into temp
                             from x in temp.DefaultIfEmpty()
                             join c in db.Product on x.ProductID equals c.ID into temps
                             from y in temps.DefaultIfEmpty()
                             where a.ID == ID 
                             select new
                             {
                                 ID = a.ID,
                                 UserName = a.UserName,
                                 ProductImg = y.ProductImg,
                                 PhoneNumber = a.PhoneNumber,
                                 Email = a.Email,
                                 WeChatNumber = a.WeChatNumber,
                                 Level = a.Level,
                                 GoodComment = (decimal?)y.GoodComment,
                                 Collections = (int?)y.Collections,
                                 CmtNum = (int?)y.CmtNum,
                                 EnterpriseName = a.EnterpriseName,
                                 EnterpriseTaxNumber = a.EnterpriseTaxNumber,
                                 Money = a.Money,
                                 Cost = a.Cost,
                                 CollectionNum = a.CollectionNum
                             }).ToList();
                return Query;
            }
        }
    }
}
