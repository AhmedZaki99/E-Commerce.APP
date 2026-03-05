using E_Commerce.App.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Domain.Entities.Product
{
    public class Vendor : BaseEntity<int>
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
        public required string PictureUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
