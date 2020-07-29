using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class ReceiveOrderComponent: IReceiveOrderComponent,IDependencys
    {
        public IReceiveOrderManager IROM { get; set; }
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool AddOrder(int pid, int num, dynamic user)
        {
            return IROM.AddOrder(pid, num,user);
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public object QueryOrder(int pid, int userid)
        {
            return IROM.QueryOrder(pid, userid);
        }
    }
}
