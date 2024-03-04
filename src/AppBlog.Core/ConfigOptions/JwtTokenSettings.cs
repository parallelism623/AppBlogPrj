using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ConfigOptions
{
    public class JwtTokenSettings
    {
        public string SecretKey { get; set; } 
        public string Issuer {  get; set; }
        public int ExpireInHours { get; set; }  
    }
}
