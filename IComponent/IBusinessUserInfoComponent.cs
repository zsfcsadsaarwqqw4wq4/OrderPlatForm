using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IComponent
{
    public interface IBusinessUserInfoComponent
    {
        /// <summary>
        /// 添加一个商家用户
        /// </summary>
        bool BusinessInfoRegister(BusinessUserInfo bui);
        /// <summary>
        /// 获取商家账户信息
        /// </summary>
        /// <returns></returns>
        BusinessUserInfo GetBUInfo(BusinessUserInfo bui);
        /// <summary>
        /// 根据用户名搜索用户对象
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        BusinessUserInfo QueryUserNameUser(string UserName);
        /// <summary>
        /// 根据电话号码搜索用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        BusinessUserInfo QueryPhoneUser(string PhoneNumber);
        /// <summary>
        /// 根据邮箱搜索用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        BusinessUserInfo QueryEmailUser(string email);
        /// <summary>
        /// 查询当前用户关联信息
        /// </summary>
        /// <returns></returns>
        object QueryBusinessUserInfo(int ID);
    }
}
