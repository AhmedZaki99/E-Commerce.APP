using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Auth
{
    public class VerifyOtpDto
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
    }
}
