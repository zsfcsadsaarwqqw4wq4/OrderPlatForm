using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface IBuyerUserInfoManager
    {
        /// <summary>
        /// 买家注册
        /// </summary>
        /// <returns></returns>
        bool BuyerUserInfoRegister(BuyerUserInfo bus);
        /// <summary>
        /// 登录验证返回一个用户对象
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        BuyerUserInfo GetBUInfo(BuyerUserInfo bus);
        /// <summary>
        /// 根据用户名搜索用户对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        BuyerUserInfo QueryUserNameUser(string UserName);
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        BuyerUserInfo QueryPhoneUser(string PhoneNumber);
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        BuyerUserInfo QueryEmailUser(string email);
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        object QueryBuyerUserInfo(int ID);
    }
}
