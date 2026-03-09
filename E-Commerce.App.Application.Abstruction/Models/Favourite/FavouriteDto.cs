using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.App.Application.Abstruction.Models.Favourite
{
    public class FavouriteDto
    {
        [Required]
        public required string Id { get; set; }

        public IEnumerable<FavouriteItemDto> Items { get; set; } = new List<FavouriteItemDto>();
    }
}
