using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.RequestData
{
    public class ResetPasswordModelRequest
    {
        public required string OldPassword { get; set; }    
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; } 
        public string Code {  get; set; }
        public string Email { get; set; }  
    }
}
