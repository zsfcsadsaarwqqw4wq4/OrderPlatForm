using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class BuyerUserInfoManager: IBuyerUserInfoManager,IDependency
    {
        /// <summary>
        /// 添加一个买家用户实体
        /// </summary>
        public bool BuyerUserInfoRegister(BuyerUserInfo bus)
        {
            using (ShopEntities db=new ShopEntities())
            {
                db.BuyerUserInfo.Add(bus);
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 获取买家个人账户信息
        /// </summary>
        /// <returns></returns>
        public BuyerUserInfo GetBUInfo(BuyerUserInfo bus)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BuyerUserInfo.SingleOrDefault(o => o.UserName.Equals(bus.UserName) && o.PassWord.Equals(bus.PassWord));
            }
        }
        /// <summary>
        /// 根据用户名搜索用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryUserNameUser(string UserName)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BuyerUserInfo.SingleOrDefault(o => o.UserName.Equals(UserName));
            }
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryPhoneUser(string PhoneNumber)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BuyerUserInfo.SingleOrDefault(o => o.PhoneNumber.Equals(PhoneNumber));
            }
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryEmailUser(string email)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.BuyerUserInfo.SingleOrDefault(o => o.Email.Equals(email));
            }
        }
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        public object QueryBuyerUserInfo(int ID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Query = (from a in db.BuyerUserInfo
                             join b in db.BuyerUser_Product on a.ID equals b.BuyerUserInfoID into temp
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
                                 WechatNumber = a.WechatNumber,
                                 Level = a.Level,
                                 GoodComment = (decimal?)y.GoodComment,
                                 Collections = (int?)y.Collections,
                                 CmtNum = (int?)y.CmtNum,
                                 EnterpriseName = a.EnterpriseName,
                                 EnterpriseTaxNumber = a.EnterpriseTaxNumber,
                                 Money = a.Money,
                                 Get = a.Get,
                                 CollectionNum = a.CollectionNum
                             }).ToList();
                return Query;
            }
        }
    }
}
