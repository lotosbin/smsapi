namespace smsapi
{
    public interface ISmsProvider
    {
        SendResult SendSms(string mobile, string content);
        SendResult tplSendSms(long tpl_id, string tpl_value, string mobile);
    }
}