﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class EmailSendContent
    {
        public string FromName { get; set; }
        public string ToName { get; set; }  
        public string FromEmail { get; set; }
        public string Content { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }    
    }
}
