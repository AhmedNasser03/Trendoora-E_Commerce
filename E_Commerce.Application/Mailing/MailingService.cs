
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace E_Commerce.Mailing
{
    public class MailingService : IMailingService
    {
		private readonly IOptions<MailSettings> _mailSettings;

		public MailingService(IOptions<MailSettings> mailSettings)
		{
			_mailSettings = mailSettings;
		}


		public void SendMail(MailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("T_ECOM", _mailSettings.Value.From));  // Changed "email" to "sender"
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<p><img src=\"~/images/logo.png\" alt=\"T_ECOM\" style=\"vertical-align:middle;\" /> <span style=\"vertical-align:middle;\">Trendoor E_Commerce</span></p>" +
                       $"{message.Content}"
            };


            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

		public async Task Send(MimeMessage message)
		{
			using var client = new SmtpClient();
			try
			{
				await client.ConnectAsync(_mailSettings.Value.SmtpServer, _mailSettings.Value.Port, SecureSocketOptions.StartTls);
				await client.AuthenticateAsync(_mailSettings.Value.Username, _mailSettings.Value.Password);
				await client.SendAsync(message);
			}
			catch (Exception ex)
			{
				// Log or handle exception
				throw new InvalidOperationException("Failed to send email", ex);
			}
			finally
			{
				await client.DisconnectAsync(true);
				client.Dispose();
			}
		}
	}

}

