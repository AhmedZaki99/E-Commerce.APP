using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Auth
{
    public class EmailSetting
    {
        public required string Host { get; set; }

        public int Port { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string DisplayName { get; set; }
        
    }
}
