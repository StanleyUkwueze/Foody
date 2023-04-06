using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Commons
{
    public class MailRequest
    {
        public string Name { get; set; }
        public string RecipientMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
    }
}
