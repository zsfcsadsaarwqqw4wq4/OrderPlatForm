using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CapitalManager : ICapitalManager,IDependency
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResponsePageData<MoneyManager> pagedata = new ResponsePageData<MoneyManager>();
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public PageDataHelper<MoneyManager> pdh = new PageDataHelper<MoneyManager>();
        /// <summary>
        /// 查询资金记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public ResponsePageData<MoneyManager> QueryMoney(int pageIndex, int pageSize, Expression<Func<MoneyManager, bool>> whereLamdba, Expression<Func<MoneyManager, int>> orderLambda)
        {
            pagedata.total= pdh.Count(whereLamdba);
            pagedata.data = pdh.GetPageData(pageIndex, pageSize, whereLamdba, orderLambda);
            return pagedata;
        }
        /// <summary>
        /// 添加充值或提现记录
        /// </summary>
        public bool AddMoney(MoneyManager mm)
        {
            bool flag = false;
            using (ShopEntities db = new ShopEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (mm.Mode == 1 || mm.Mode==4 || mm.Mode==5)
                        {
                            var data = db.BusinessUserInfo.SingleOrDefault(o => o.UserName.Equals(mm.UserName));
                            if (data != null)
                            {
                                data.Money = data.Money + mm.Price;
                            }
                            db.SaveChanges();
                        }
                        if (mm.Mode==2 ||mm.Mode==3)
                        {
                            var data = db.BuyerUserInfo.SingleOrDefault(o => o.UserName.Equals(mm.UserName));
                            if (data != null)
                            {
                                data.Money = data.Money + mm.Price;
                            }
                            db.SaveChanges();
                        }
                        db.MoneyManager.Add(mm);
                        db.SaveChanges();
                        transaction.Commit();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                    return flag;

                }
            }
        }
        /// <summary>
        /// 根据用户名搜索用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public MoneyManager QueryUserNameUser(string UserName)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.MoneyManager.SingleOrDefault(o => o.UserName.Equals(UserName));
            }
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public MoneyManager QueryPhoneUser(string PhoneNumber)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.MoneyManager.SingleOrDefault(o => o.PhoneNumber.Equals(PhoneNumber));
            }
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public MoneyManager QueryEmailUser(string email)
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.MoneyManager.SingleOrDefault(o => o.Email.Equals(email));
            }
        }
    }
}
