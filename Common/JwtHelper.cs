using Domain;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class JwtHelper
    {
        const string secret = "RHKJ";

        public static string CreateToken(dynamic Bus, DateTime time)
        {
            try
            {
                AuthInfo info = new AuthInfo { UserName = Bus.UserName, ID = Bus.ID, Iat = time ,Level=Bus.Level ,EndTime =time.AddDays(7)};
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                return encoder.Encode(info, secret);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static AuthInfo GetJwtDecode(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var userInfo = decoder.DecodeToObject<AuthInfo>(token, secret, verify: true);
            return userInfo;
        }
    }
    public class AuthInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// jwt的签发时间
        /// </summary>
        public DateTime Iat { get; set; }
        /// <summary>
        /// jwt的过期时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 表示登录平台 0:商家登录,1:个人买家登录,2:团体买家登录
        /// </summary>
        public int Level { get; set; }
    }
}
