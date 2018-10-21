using System.Net.Mail;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(
            string to,
            string subject = null,
            string body = null,
            Attachment attachment = null,
            string from = null,
            string cc = null,
            string bcc = null);
    }
}