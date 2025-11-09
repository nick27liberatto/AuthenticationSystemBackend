namespace Infrastructure.Services
{
    using Application.Interfaces;
    using Infrastructure.Configurations;
    using Microsoft.Extensions.Options;
    using System.Net;
    using System.Net.Mail;

    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        public SmtpEmailService(IOptions<EmailSettings> options) => _settings = options.Value;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("To, subject, and body must be provided");

            try
            {
                using var smtp = new SmtpClient(_settings.Smtp.Host, _settings.Smtp.Port)
                {
                    Credentials = new NetworkCredential(_settings.Smtp.User, _settings.Smtp.Pass),
                    EnableSsl = true
                };

                var mail = new MailMessage(_settings.From, to, subject, body) { IsBodyHtml = true };

                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending e-mail to {to}", ex);
            }
        }
    }
}
