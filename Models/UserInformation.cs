﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class UserInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UserInformationFromAPI
    {
        public string ResponseMessage { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string UID { get; set; }
        
    }
}
