using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
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

        public SendResult tplSendSms(long tpl_id, string tpl_value, string mobile)
        {
            throw new System.NotImplementedException();
        }

        public SendResult SendSms(string username, string password, string mobile, string content)
        {
            const string url = "http://www.smsbao.com/sms";
            var request = new SyncHttpRequest();
            var get = request.HttpGet(url, new List<APIParameter>()
            {
                new APIParameter("u",username),
                new APIParameter("p",MD5Encoding(password)),
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
        } /// <summary>
        /// MD5 加密字符串
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <returns>加密后字符串</returns>
        private static string MD5Encoding(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}