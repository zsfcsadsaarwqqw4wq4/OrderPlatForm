﻿using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OrderPlatForm.App_Start
{
    public class RedisHelper
    {
        /// <summary>
        /// 获取Redis连接ip地址
        /// </summary>
        private static string RedisPath = ConfigurationManager.AppSettings["RedisPath"];
        /// <summary>
        /// 获取需要连接的端口
        /// </summary>
        private static int RedisPort = Convert.ToInt32(ConfigurationManager.AppSettings["RedisPort"]);
        /// <summary>
        /// 获取某个key的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long endtime(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                long entime = redisclient.PTtl(key);
                return entime;
            }
        }
        /// <summary>
        /// 使用多个多个redis服务器，均衡负载项目
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <returns></returns>
        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //支持读写分离，均衡负债
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxReadPoolSize = 10,//读取最大连接数
                MaxWritePoolSize = 10,//写入最大连接数
                AutoStart = true
            });
        }
        #region string类型,可以包含任何数据,比如图片二进制或序列化的对象
        /// <summary>
        /// 保存一个对象，该对象会被序列化并设置过期时间,Set方法会自动将对象序列化成一个string类型的json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value, TimeSpan time)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<T>(key, value, time);
                return res;
            }
        }
        /// <summary>
        /// 保存一个对象，该对象会被序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<T>(key, value);
                return res;
            }
        }        
        /// <summary>
        /// 获取一个object对象，该对象是通过反序列化得到
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDictionary<string, object> StringGet(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                ///该方法会获取到反序列化对象
                var data = redisclient.GetValue(key);
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(data);
                }
                return null;
            }
        }
        /// <summary>
        /// 获取key，返回byte格式
        /// </summary>
        /// <returns></returns>
        public byte[] GetValueByte(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                byte[] value = redisclient.Get(key);
                return value;
            }
        }
        /// <summary>
        /// 存储string类型数据有过期时间
        /// </summary>
        public bool SetString(string key, string value, TimeSpan time)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<string>(key, value, time);
                return res;
            }
        }
        /// <summary>
        /// 存储string类型数据有过期时间
        /// </summary>
        public bool SetString(string key, string value, DateTime time)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<string>(key, value, time);
                return res;
            }
        }
        /// <summary>
        /// 设置多个key/value
        /// </summary>
        /// <param name="dic"></param>
        public void Set(Dictionary<string, string> dic)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                redisclient.SetAll(dic);
            }
        }
        #region 追加
        /// <summary>
        /// 在原有key的value值之后追加value
        /// </summary>
        public long Append(string key, string value)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
               return redisclient.AppendToValue(key, value);
            }
        }
        #endregion
        /// <summary>
        /// 存储string类型数据无过期时间
        /// </summary>
        public bool SetString(string key, string value)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<string>(key, value);
                return res;
            }
        }
        /// <summary>
        /// 存储int类型数据无过期时间
        /// </summary>
        public bool SetInt(string key, int value)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                bool res = redisclient.Set<int>(key, value);
                return res;
            }
        }
        #region 获取 
        /// <summary>
        /// 根据key得到value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            //创建Redis连接对象
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                //存放string类型数据到内存中
                string res = redisclient.GetValue(key);
                return res;
            }
        }
        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        public List<string> Get(List<string> keys)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetValues(keys);
            }
        }
        #endregion
        /// <summary>
        /// 根据key删除string类型的值
        /// </summary>
        /// <param name="key"></param>
        public bool DeleteString(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                bool res = redisclient.Remove(key);
                return res;
            }
        }

        #endregion
        #region hash类型是一个string类型的field和value的映射表.hash特别适合存储对象,因为它占用的内存更小。
        /// <summary>
        /// 写入hash类型数据
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetEntryInHash(string hashId, string key, string value)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.SetEntryInHash(hashId, key, value);
            };
        }
        /// <summary>
        /// hash类型获取key
        /// </summary>
        /// <param name="hashId"></param>
        public List<string> GetHashKeys(string hashId)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetHashKeys(hashId);
            };
        }
        /// <summary>
        /// hash类型获取值
        /// </summary>
        /// <param name="hashId"></param>
        public List<string> GetHashValues(string hashId)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetHashValues(hashId);
            };
        }
        /// <summary>
        /// 获取hash类型的值，根据hashid和key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueFromHash(string hashId, string key )
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetValueFromHash(hashId, key);
            };
        }
        /// <summary>
        /// 获取指定hashid的数据字典
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAllEntriesFromHash(string hashId)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetAllEntriesFromHash(hashId);
            };
        }
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllKeys()
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetAllKeys();
            };
        }
        #endregion
        /// <summary>
        /// 存储List类型数据指定key值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetList(string key, List<string> value)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                redisclient.AddRangeToList(key, value);
            }
        }
        /// <summary>
        /// 获取所有list列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> GetList(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetAllItemsFromList(key);
            }
        }
        /// <summary>
        /// 根据key和index索引对应的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetIndexItem(string key, int index)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                var item = redisclient.GetItemFromList(key, index);
                return item;
            }
        }
        /// <summary>
        /// 添加Set集合
        /// </summary>
        /// <param name="setId">集合名</param>
        /// <param name="item">集合值</param>
        public void AddItemToSet(string setId,string item)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                redisclient.AddItemToSet(setId, item);
            }
        }
        public HashSet<string> GetAllItemsFromSet(string setId)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                return redisclient.GetAllItemsFromSet(setId);
            }
        }
        /// <summary>
        /// 删除指定集合
        /// </summary>
        /// <param name="key">需要删除的集合名</param>
        public void ClearAll(string key)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                var list = redisclient.Lists[key];
                list.Clear();
            }
        }
        /// <summary>
        /// 删除指定key的值
        /// </summary>
        /// <param name="key">需要删除的集合名</param>
        /// <param name="value">集合指定的key</param>
        public void Remove(string key, string value)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                var list = redisclient.Lists[key];
                list.Remove(value);
            }
        }
        /// <summary>
        /// 删除指定索引的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        public void RemoveAt(string key, int index)
        {
            using (RedisClient redisclient = new RedisClient(RedisPath, RedisPort, "123456"))
            {
                var list = redisclient.Lists[key];
                list.RemoveAt(index);
            }
        }

    }
}