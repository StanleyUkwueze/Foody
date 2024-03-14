using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public bool Status { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
