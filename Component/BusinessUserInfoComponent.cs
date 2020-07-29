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
    public class BusinessUserInfoComponent : IBusinessUserInfoComponent, IDependencys
    {
        public IBusinessUserInfoManager IBUIM { get; set; }
        /// <summary>
        /// 添加一个商家用户
        /// </summary>
        public bool BusinessInfoRegister(BusinessUserInfo bui)
        {
            return IBUIM.BusinessInfoRegister(bui);
        }
        /// <summary>
        /// 获取商家账户信息
        /// </summary>
        /// <returns></returns>
        public BusinessUserInfo GetBUInfo(BusinessUserInfo bui)
        {
            return IBUIM.GetBUInfo(bui);
        }
        /// <summary>
        /// 根据用户名搜索用户对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryUserNameUser(string UserName)
        {
            return IBUIM.QueryUserNameUser(UserName);
        }
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryPhoneUser(string UserName)
        {
            return IBUIM.QueryPhoneUser(UserName);
        }
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BusinessUserInfo QueryEmailUser(string UserName)
        {
            return IBUIM.QueryEmailUser(UserName);
        }
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        public object QueryBusinessUserInfo(int ID)
        {
            return IBUIM.QueryBusinessUserInfo(ID);
        }
    }
}
