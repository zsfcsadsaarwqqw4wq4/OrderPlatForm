using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IComponent
{
    public interface IReceiveOrderComponent
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        bool AddOrder(int pid, int num, dynamic user);
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        object QueryOrder(int pid, int userid);

    }
}
