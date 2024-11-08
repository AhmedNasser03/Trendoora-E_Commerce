using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Authentication
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OTP { get; set; }
    }
}
