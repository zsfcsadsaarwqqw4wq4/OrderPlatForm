using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Tools
    {
        private static object lockIPN = new object();
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        public static void WriteLog(string path, string log)
        {
            lock (lockIPN)
            {
                string domain = AppDomain.CurrentDomain.BaseDirectory;
                if (!path.StartsWith(domain))
                {
                    path = domain + path;
                }
                File.AppendAllText(path, log);
            }
        }
        /// <summary>
        /// Linq按多项去重复
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source">去重对象</param>
        /// <param name="keySelector">表达式（例：aa => new { aa.UseName, aa.UsId }）</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }

        /// <summary>
        /// 获取对象指定的属性值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">输出的值</param>
        /// <returns>属性值</returns>
        /// <exception cref="NullReferenceException">当指定的属性值为空，或不存在时抛出异常</exception>
        public static void GetPropertyValue<T>(this object entity, string propertyName, out T value)
        {
            value = (T)entity.GetType().GetProperty(propertyName).GetValue(entity);
        }

        /// <summary>
        /// 获取对象指定的属性值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        /// <exception cref="NullReferenceException">当指定的属性值为空，或不存在时抛出异常</exception>
        public static T GetPropertyValue<T>(this object entity, string propertyName)
        {
            entity.GetPropertyValue(propertyName, out T temp);
            return temp;
        }

        /// <summary>
        /// 获取对象指定的属性值
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(this object entity, string propertyName) => entity.GetType().GetProperty(propertyName).GetValue(entity);
    }
}
