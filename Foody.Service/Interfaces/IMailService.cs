using AutoMapper.Internal;
using Foody.Commons;
using Foody.Service.Implementations;
using Mailjet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Interfaces
{
    public interface IMailService
    {
        // Task SendEmailAsync(MailRequest mailRequest);
        Task<string> SendEmailAsync(EmailMessage emailMessage);
    }
}
