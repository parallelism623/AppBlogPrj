using Core.Models;

namespace AppBlog.Api.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailSendContent content);
    }
}
