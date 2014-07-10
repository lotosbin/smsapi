using System;
using smsapi.Providers;

namespace smsapi
{
    public static class SmsService
    {
        public static SendResult SendSms(SmsProviders provider, string mobile, string content)
        {
            switch (provider)
            {
                case SmsProviders.Yunpian:
                    throw new Exception("不支持的提供者类型");
                    break;
                case SmsProviders.Smsbao:
                    return new SmsbaoSMSProvider().SendSms(mobile, content);
                    break;
                default:
                    throw new Exception("不支持的提供者类型");
                    break;
            }
        }
    }
}