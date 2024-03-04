using Core.ConfigOptions;
using Core.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace AppBlog.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(EmailSendContent content)
        {
            var message = new MimeMessage();
            var emailFrom = content.FromEmail ?? _emailSettings.SenderEmail;
            var emailTo = content.ToEmail ?? string.Empty;
            var nameFrom = content.FromName ?? _emailSettings.SenderName;
            var nameTo = content.ToName ?? string.Empty;
            message.From.Add(new MailboxAddress(nameFrom, emailFrom));
            message.To.Add(new MailboxAddress(nameTo, emailTo));
            message.Subject = content.Subject;
            var buildBody = new BodyBuilder();
            var textBody = content.Content;
            buildBody.TextBody = textBody;
            message.Body = buildBody.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer,587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
