using System.ComponentModel.DataAnnotations;

namespace E_Commerce.App.Application.Abstruction.Models.Favourite
{
    public class FavouriteItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public required string ProductName { get; set; }

        public string? PictureUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Vendor { get; set; }

        public string? Category { get; set; }
    }
}
