using Domain;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class BuyerUserInfoComponent:IBuyerUserInfoComponent,IDependencys
    {
        public IBuyerUserInfoManager IBUIM { get; set; }
        /// <summary>
        /// 买家注册
        /// </summary>
        /// <returns></returns>
        public bool BuyerUserInfoRegister(BuyerUserInfo bus)
        {
            return IBUIM.BuyerUserInfoRegister(bus);

        }
        /// <summary>
        /// 获取买家个人账户信息
        /// </summary>
        /// <returns></returns>
        public BuyerUserInfo GetBUInfo(BuyerUserInfo bus)
        {
            return IBUIM.GetBUInfo(bus);
        }
        /// <summary>
        /// 根据用户名搜索用户对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryUserNameUser(string UserName)
        {
            return IBUIM.QueryUserNameUser(UserName);
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryPhoneUser(string UserName)
        {
            return IBUIM.QueryPhoneUser(UserName);
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BuyerUserInfo QueryEmailUser(string UserName)
        {
            return IBUIM.QueryEmailUser(UserName);
        }
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        public object QueryBuyerUserInfo(int ID)
        {
            return IBUIM.QueryBuyerUserInfo(ID);
        }
    }
}
