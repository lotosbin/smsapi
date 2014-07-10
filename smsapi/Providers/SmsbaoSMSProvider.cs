using System.Collections.Generic;
using System.Configuration;
using Binbin.HttpHelper;

namespace smsapi.Providers
{
    /// <summary>
    /// http://www.smsbao.com/
    /// 错误代码表
    /// 30：密码错误 
    /// 40：账号不存在
    /// 41：余额不足
    /// 42：帐号过期
    /// 43：IP地址限制
    /// 50：内容含有敏感词
    /// 51：手机号码不正确
    /// </summary>
    /// 
    public class SmsbaoSMSProvider : ISmsProvider
    {
        public SendResult SendSms(string mobile, string content)
        {
            var username = ConfigurationManager.AppSettings["smsbao_username"];
            if (string.IsNullOrEmpty(username))
            {
                throw new ConfigurationErrorsException("smsbao_username");
            }
            var password = ConfigurationManager.AppSettings["smsbao_password"];
            if (string.IsNullOrEmpty(password))
            {
                throw new ConfigurationErrorsException("smsbao_password");
            }
            return SendSms(username, password, mobile, content);
        }
        public SendResult SendSms(string username, string password, string mobile, string content)
        {
            const string url = "http://www.smsbao.com/sms";
            var request = new SyncHttpRequest();
            var get = request.HttpGet(url, new List<APIParameter>()
            {
                new APIParameter("u",username),
                new APIParameter("p",password),
                new APIParameter("m",mobile),
                new APIParameter("c",content),
            });
            if (get[0] == '0')
            {
                return new SendResult()
                {
                    Success = true,
                    Message = "",
                };
            }
            return new SendResult()
            {
                Success = false,
                Message = get,
            };
        }
    }
}