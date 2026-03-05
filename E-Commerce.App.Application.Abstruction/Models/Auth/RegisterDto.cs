using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Auth
{
    public class RegisterDto
    {
        [Required]
        public required string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [Phone]
        public required string Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#%^&()_+}{"";:'?/<>.,]).*$",
        ErrorMessage = "Password must have 1 UpperCase, 1 LowerCase, 1 number, 1 non-alphanumeric character, and be between 6 to 10 characters long.")]
        public required string Password { get; set; }
    }
}
