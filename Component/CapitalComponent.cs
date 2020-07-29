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
    public class CapitalComponent : ICapitalComponent, IDependencys
    {
        public ICapitalManager ICM { get; set; }
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
            return ICM.QueryMoney(pageIndex, pageSize, whereLamdba, orderLambda);
        }
        /// <summary>
        /// 添加充值或提现记录
        /// </summary>
        public bool AddMoney(MoneyManager mm)
        {
            return ICM.AddMoney(mm);
        }
        /// <summary>
        /// 根据用户名搜索用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public MoneyManager QueryUserNameUser(string UserName)
        {
            return ICM.QueryUserNameUser(UserName);
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public MoneyManager QueryPhoneUser(string PhoneNumber)
        {
            return ICM.QueryPhoneUser(PhoneNumber);
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public MoneyManager QueryEmailUser(string email)
        {
            return ICM.QueryEmailUser(email);
        }
    }
}
