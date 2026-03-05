using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Domain.Entities.Identity
{
    public class ApplicationsUser :IdentityUser
    {
        public required string DisableName { get; set; }

        public virtual Address? Address { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpire { get; set; }
    }
}
