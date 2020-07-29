using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ResponsePageData<T>
    {
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }
        /// <summary>
        /// 返回父级和子级数据
        /// </summary>
        public List<ResponsePageDatas<T>> datas { get; set; }
    }
    public class ResponsePageDatas<T>
    {
        /// <summary>
        /// 分页查询的父级数据
        /// </summary>
        public T fdata { get; set; }
        /// <summary>
        /// 分页查询的子级数据
        /// </summary>
        public List<T> data { get; set; }
    }
    public class ResultPageData<T>
    {
        /// <summary>
        /// 返回记录总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T data { get; set; }
    }
}
