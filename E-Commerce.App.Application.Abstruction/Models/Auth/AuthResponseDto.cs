using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Auth
{
    public class AuthResponseDto
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
