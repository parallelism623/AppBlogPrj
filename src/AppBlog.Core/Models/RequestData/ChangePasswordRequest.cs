using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.RequestData
{
    public class ChangePasswordRequest
    {

        public required string OldPassword { get; set; }
        public required string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirmpassword is not match with password")]
        public required string ConfirmPassword { get; set; }    
    }
}
