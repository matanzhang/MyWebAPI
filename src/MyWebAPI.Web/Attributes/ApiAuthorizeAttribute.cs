using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MyWebAPI.Entities;

namespace MyWebAPI.Web.Attributes
{
    public class ApiAuthorizeAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                var authHeader = from t in actionContext.Request.Headers where t.Key == "auth" select t.Value.FirstOrDefault();
                var enumerable = authHeader as string[] ?? authHeader.ToArray();
                string token = enumerable.FirstOrDefault();
                if (string.IsNullOrEmpty(enumerable.FirstOrDefault())) return false;
                const string secretKey = "matanzhang";//口令加密秘钥（应该写到配置文件中）
                byte[] key = Encoding.UTF8.GetBytes(secretKey);
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                //解密
                var authInfo = decoder.DecodeToObject<AuthInfo>(token, key, verify: true);
                if (authInfo != null)
                {
                    //判断口令过期时间
                    if (authInfo.Expires < DateTime.Now)
                    {
                        return false;
                    }
                    actionContext.RequestContext.RouteData.Values.Add("auth", authInfo);
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }
        /// <summary>
        /// 处理授权失败的请求
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var erModel = new HttpResult()
            {
                Success = false,
                Message = "身份认证不正确！"
            };
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, erModel, "application/json");
        }
    }
}