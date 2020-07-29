using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumHelper
    {
        public enum ExamineEnum
        {
            [Description("未审核")]//描述
            System = 0,
            [Description("已审核")]
            Procurement = 1,
        }
        /// <summary>
        /// 接单权限
        /// </summary>
        public enum PowerEnum
        {
            [Description("卖家")]
            One = 1,
            [Description("买家")]
            Two = 2,
            [Description("团队买家")]
            Three = 3,
            [Description("团队卖家")]
            Four = 4,
            [Description("平台方")]
            Five = 5
        }
        public enum UserEnum
        {
            [Description("用户名")]
            Zreo = 0,
            [Description("邮箱")]
            One = 1,
            [Description("手机号")]
            Two = 2
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public enum StatusCode
        {
            [Description("失败")]
            ErrorCode = 404,
            TokenExpired = 410
        }
        /// <summary>
        /// 获取枚举的描述属性
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {

            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);  //获取描述属性
            if (objs == null || objs.Length == 0)  //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }
}
