using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using EatAsYouGoApi.Helper;
using EatAsYouGoApi.Services.Interfaces;

namespace EatAsYouGoApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogService _logService;
        private readonly string _smtpHost;

        public EmailService(ILogService logService)
        {
            _logService = logService;
            _smtpHost = Config.SmtpHost;
        }

        private const string DefaultFromMailAddress = "team@eatasyougo.com";

        public bool SendEmail(
            string to,
            string subject = "",
            string body = "",
            Attachment attachment = null,
            string from = null,
            string cc = null,
            string bcc = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(from))
                    from = DefaultFromMailAddress;

                if (string.IsNullOrWhiteSpace(to))
                    throw new ArgumentNullException(nameof(to), "'To' email address cannot be null or empty");

                using (var smtpClient = new SmtpClient(_smtpHost))
                {
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(from);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.BodyEncoding = Encoding.Default;

                        to.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList()
                            .ForEach(email =>
                            {
                                if (ValidateEmailAddress(email))
                                    mailMessage.To.Add(email);
                            });

                        if (attachment != null)
                            mailMessage.Attachments.Add(attachment);

                        if (!string.IsNullOrWhiteSpace(bcc))
                        {
                            bcc.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries)
                                .ToList()
                                .ForEach(email =>
                                {
                                    if (ValidateEmailAddress(email))
                                        mailMessage.Bcc.Add(email);
                                });
                        }

                        if (!string.IsNullOrWhiteSpace(cc))
                        {
                            cc.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList()
                                .ForEach(email =>
                                {
                                    if (ValidateEmailAddress(email))
                                    mailMessage.CC.Add(email);
                            });
                        }

                        smtpClient.Send(mailMessage);
                        _logService.Info($"Email sent successfully to: {GetEmailAddresses(mailMessage)}");
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                _logService.Error($"Failed to send email: {exception.Message}", exception);
                return false;
            }
        }

        private static string GetEmailAddresses(MailMessage mailMessage)
        {
            var addresses = mailMessage.To.Select(m => m.Address).Aggregate((a, b) => $"{a},{b}");

            if (mailMessage.CC.Any())
                addresses += "," + mailMessage.CC.Select(m => m.Address).Aggregate((a, b) => $"{a},{b}");

            if (mailMessage.Bcc.Any())
                addresses += "," + mailMessage.Bcc.Select(m => m.Address).Aggregate((a, b) => $"{a},{b}");

            return addresses;
        }

        private static bool ValidateEmailAddress(string email)
        {
            const string emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                        + "@"
                                        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, emailPattern);
        }
    }
}
