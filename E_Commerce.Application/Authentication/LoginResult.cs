﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Authentication
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public LoginErrorType? ErrorType { get; set; }
        public List<string> roles { get; set; }
    }

    public enum LoginErrorType
    {
        InvalidPassword,
        UserNotFound,
        EmailNotConfirmed
    }
}
