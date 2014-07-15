using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Binbin.HttpHelper;

namespace smsapi.Providers
{
    /// <summary>
    /// http://www.smsbao.com/
    /// ��������
    /// 30��������� 
    /// 40���˺Ų�����
    /// 41������
    /// 42���ʺŹ���
    /// 43��IP��ַ����
    /// 50�����ݺ������д�
    /// 51���ֻ����벻��ȷ
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
        /// MD5 �����ַ���
        /// </summary>
        /// <param name="rawPass">Դ�ַ���</param>
        /// <returns>���ܺ��ַ���</returns>
        private static string MD5Encoding(string rawPass)
        {
            // ����MD5���Ĭ��ʵ����MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // ��ʮ�����Ƹ�ʽ��ʽ��
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}