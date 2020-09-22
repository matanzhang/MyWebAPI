using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MyWebAPI.Entities;

namespace MyWebAPI.Web.Controllers
{
    [RoutePrefix("api/System")]
    public class SystemController : ApiController
    {
        [HttpPost, Route("Login")]
        public HttpResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null) return new HttpResult() { Success = false, Message = "登录信息为空！" };

            #region 通过数据库判断登录信息是否正确（这里简化判断）

            if (loginRequest.UserId != "admin" || loginRequest.Password != "admin")
            {
                return new HttpResult() { Success = false, Message = "用户名和密码不正确！" };
            }
            #endregion
            AuthInfo authInfo = new AuthInfo()
            {
                UserId = loginRequest.UserId,
                Expires = DateTime.Now.AddDays(1)
            };
            const string secretKey = "matanzhang";//口令加密秘钥（应该写到配置文件中）
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//加密方式
            IJsonSerializer serializer = new JsonNetSerializer();//序列化Json
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//base64加解密
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);//JWT编码
            var token = encoder.Encode(authInfo, key);//生成令牌

            return new HttpResult() { Success = true, Data = token,Message = "登录成功！"};
        }
    }
}