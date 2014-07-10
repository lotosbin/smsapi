namespace smsapi
{
    public interface ISmsProvider
    {
        SendResult SendSms(string mobile, string content);
    }
}