using Foody.Commons;
using Foody.Model.Settings;
using Foody.Service.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Service.Implementations
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;
        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<MailjetResponse> SendEmailAsync(string email, string subject, string htmlMessage)
        {
          return await Execute(email, subject, htmlMessage);
        }
        public async Task<MailjetResponse> Execute(string email, string subject, string body)
        {


            //var filePath = Directory.GetCurrentDirectory() + "\\MailPage\\mail.html";

            //var streamReader = new StreamReader(filePath);

            //var mailText = streamReader.ReadToEnd();
            //streamReader.Close();
            //mailText = mailText.Replace("[username]", "Stanley")
            //                    .Replace("[Message]", email);

            //body = mailText;

            MailjetClient client = new MailjetClient("88e3288bab40b64e7db026b5e2187bb1" ,"70e2afa525c204439749fa7e711c5b2f")
            {
                //Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "Sender",
       new JObject {
        {"Email", "stanleyjekwu16@gmail.com"},
        {"Name", "Stanley"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
            {
          "Email",
         email
         }, {
          "Name",
         "STanley"
         }
        }
       }
      }, {
       "Subject",
     subject
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
        body
      }
     }
             });
          var res =  await client.PostAsync(request);
            return res;
        }

    }
}

      

