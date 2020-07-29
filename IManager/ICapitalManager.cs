using Common;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface ICapitalManager
    {
        /// <summary>
        /// 查询资金记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLamdba"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        ResponsePageData<MoneyManager> QueryMoney(int pageIndex, int pageSize, Expression<Func<MoneyManager, bool>> whereLamdba, Expression<Func<MoneyManager, int>> orderLambda);
        /// <summary>
        /// 添加充值或提现记录
        /// </summary>
        bool AddMoney(MoneyManager mm);
        /// <summary>
        /// 根据用户名搜索用户对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        MoneyManager QueryUserNameUser(string UserName);
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        MoneyManager QueryPhoneUser(string PhoneNumber);
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        MoneyManager QueryEmailUser(string email);
    }
}
