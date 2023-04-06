using AutoMapper.Internal;
using Foody.Commons;
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

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
