using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.RequestData
{
    public class LoginAccountRequest
    {
       
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
