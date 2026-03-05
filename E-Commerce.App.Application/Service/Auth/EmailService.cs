using E_Commerce.App.Application.Abstruction.Models.Auth;
using E_Commerce.App.Application.Abstruction.Services.Auth;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;

namespace E_Commerce.App.Application.Service.Auth
{
    internal class EmailService(IOptions<EmailSetting> _Options) : IEmailService
    {
        public void SendEmail(string to, string subject, string boody)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_Options.Value.Email),
                Subject = subject,
            };

            mail.To.Add(MailboxAddress.Parse(to));
            mail.From.Add(MailboxAddress.Parse(_Options.Value.Email));

            var builder = new BodyBuilder();
            builder.TextBody = boody;
            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_Options.Value.Host, _Options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_Options.Value.Email, _Options.Value.Password);
            smtp.Send(mail);

            smtp.Disconnect(true);
        }
    }
}
