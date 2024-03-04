﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Auth
{
    public class TokenRequest
    {
        public required string Token { get; set; }    
        public required string RefreshToken { get; set; }
    }
}
