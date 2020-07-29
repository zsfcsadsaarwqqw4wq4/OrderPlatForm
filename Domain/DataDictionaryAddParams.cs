using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// 添加数据字典参数
    /// </summary>
    public class DataDictionaryAddParams
    {
        /// <summary>
        /// 数据字典键值
        /// </summary>
        [JsonProperty(Required = Required.Always, PropertyName = "key")]
        public UString Key { get; set; }

        /// <summary>
        /// 数据字典值
        /// </summary>
        [JsonProperty(Required = Required.Always, PropertyName = "value")]
        public UString Value { get; set; }

        /// <summary>
        /// 父级字典ID
        /// </summary>
        [JsonProperty(PropertyName = "FID")]
        public int? FID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [JsonProperty(PropertyName = "Color")]
        public string Color { get; set; }

        /// <summary>
        /// 自定义参数
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public string Custom { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 删除字典参数
    /// </summary>
    public class DeleteDataDictionaryParams
    {
        [JsonProperty(PropertyName = "ID", Required = Required.Always)]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "FID")]
        public int? FID { get; set; }
    }

    /// <summary>
    /// 修改字典参数
    /// </summary>
    public class EdtiDataDictionary
    {
        [JsonProperty(PropertyName = "dictionaryID", Required = Required.Always)]
        public int DictionaryID { get; set; }
        public int? fDictionaryID { get; set; }
        public string value { get; set; }
        public string key { get; set; }
        public string icon { get; set; }
        public string Color { get; set; }
        public string custom { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
    /// <summary>
    /// 不可为空的字符串类型
    /// </summary>
    public class UString
    {
        private readonly string str;
        public UString(string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                throw new OperateException("此字段不能为空");
            }
            this.str = str;
        }
        public static implicit operator UString(string a) => new UString(a);
        public static implicit operator string(UString v) => v.str;
        public override string ToString() => str;
    }
}
