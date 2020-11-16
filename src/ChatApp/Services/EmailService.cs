using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System;

namespace ChatApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailConfig> _config;

        public EmailService(IOptions<EmailConfig> config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Chat administration", _config.Value.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config.Value.Host, _config.Value.Port, _config.Value.Ssl);
                await client.AuthenticateAsync(_config.Value.Email, _config.Value.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(_config.Value.Disconnect);
            }
        }
    }
}