namespace E_Commerce.Mailing
{
    public interface IMailingService
    {
        void SendMail(MailMessage message);
    }
}
