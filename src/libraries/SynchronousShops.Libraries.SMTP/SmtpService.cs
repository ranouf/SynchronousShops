using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.SMTP.Configuration;
using SynchronousShops.Libraries.SMTP.Extensions;
using SynchronousShops.Libraries.SMTP.SmtpClients;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SynchronousShops.Libraries.SMTP
{
    public class SmtpService : ISmtpService
    {
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpService> _logger;

        public SmtpService(
            [NotNull] ISmtpClientFactory smtpClientFactory,
            [NotNull] IOptions<SmtpSettings> smtpSettings,
            ILogger<SmtpService> logger
        )
        {
            _smtpClientFactory = smtpClientFactory;
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = GenerateEmail(to, subject, body);

            try
            {
                using (var smtpClient = _smtpClientFactory.CreateSmtpClient())
                {
                    await smtpClient.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.EnableSsl);

                    await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

                    await smtpClient.SendAsync(email);
                }

                _logger.LogInformation($"[{nameof(SmtpService)}] Email has been sent. email: {email.ToJson()}");
            }
            catch (Exception e)
            {
                _logger.LogError($"[{nameof(SmtpService)}] Email sending failed. Subject: {email.Subject}, To: {email.To}", e);
                throw;
            }
        }

        private MimeMessage GenerateEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.ProjectName, _smtpSettings.DefaultFrom));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            return email;
        }
    }
}
