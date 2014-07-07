using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Caidan.Infrastructures
{
    public class YunpianSMSProvider
    {
        /**
        * ����http��ַ
        */
        private static string BASE_URI = "http://yunpian.com";
        /**
        * ����汾��
        */
        private static string VERSION = "v1";
        /**
        * ���˻���Ϣ��http��ַ
        */
        private static readonly string URI_GET_USER_INFO = BASE_URI + "/" + VERSION + "/user/get.json";
        /**
        * ͨ�ýӿڷ����ŵ�http��ַ
        */
        private static readonly string URI_SEND_SMS = BASE_URI + "/" + VERSION + "/sms/send.json";
        /**
        * ģ��ӿڶ��Žӿڵ�http��ַ
        */
        private static readonly string URI_TPL_SEND_SMS = BASE_URI + "/" + VERSION + "/sms/tpl_send.json";

        /**
        * ȡ�˻���Ϣ
        * @return json��ʽ�ַ���
        */

        public static string getUserInfo(string apikey)
        {
            WebRequest req = WebRequest.Create(URI_GET_USER_INFO + "?apikey=" + apikey);
            WebResponse resp = req.GetResponse();
            var sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        /**
        * ͨ�ýӿڷ�����
        * @param text���������ݡ�
        * @param mobile�����ܵ��ֻ���
        * @return json��ʽ�ַ���
        */

        public static string sendSms(string apikey, string text, string mobile)
        {
            string parameter = "apikey=" + apikey + "&text=" + text + "&mobile=" + mobile;
            WebRequest req = WebRequest.Create(URI_SEND_SMS);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(parameter); //�����������Ϊutf8
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            var sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        /**
        * ģ��ӿڷ�����
        * @param tpl_id ģ��id
        * @param tpl_value ģ�����ֵ
        * @param mobile�����ܵ��ֻ���
        * @return json��ʽ�ַ���
        */

        public static string tplSendSms(string apikey, long tpl_id, string tpl_value, string mobile)
        {
            string encodedTplValue = Uri.EscapeDataString(tpl_value);
            string parameter = "apikey=" + apikey + "&tpl_id=" + tpl_id + "&tpl_value=" + encodedTplValue + "&mobile=" + mobile;
            WebRequest req = WebRequest.Create(URI_TPL_SEND_SMS);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(parameter); //�����������Ϊutf8
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            var sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static void tplSendSms(string appApikey, string notifyTelephone, int tplId, Dictionary<string, string> dictionary)
        {
            var strings = dictionary.Select(d => String.Format("#{0}#={1}", d.Key, d.Value))
                                    .ToArray();
            var format = String.Join("&", strings);
            var s = tplSendSms(appApikey, tplId, format, notifyTelephone);
        }
    }
}