using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Model.Settings
{
    public class JWTData
    {
        public const string Data = "JWTConfigurations";
        public TimeSpan TokenLifeTime { get; set; }

        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
