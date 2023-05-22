﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Models.Authentication
{
    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string Token { get; set; }
    }
}
