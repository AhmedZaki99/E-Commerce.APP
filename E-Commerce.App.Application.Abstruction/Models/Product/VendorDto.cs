using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Product
{
    public class VendorDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public  string? PictureUrl { get; set; }

        public required string Address { get; set; }
    }
}
