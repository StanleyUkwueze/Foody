using Foody.Commons;
using Foody.Model.Settings;
using Foody.Service.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;
        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> SendEmailAsync(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(Username, Email));

            email.To.Add(MailboxAddress.Parse(emailMessage.To));

            email.Subject = emailMessage.Subject;
            var builder = new BodyBuilder();


            builder.HtmlBody = emailMessage.Body;
            email.Body = builder.ToMessageBody();


            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(Host, Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(Email, Password);
            var result = await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return result;
        }

    }
}

      

