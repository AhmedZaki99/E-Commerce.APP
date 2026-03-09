using System.ComponentModel.DataAnnotations;

namespace E_Commerce.App.Application.Abstruction.Models.Basket
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string ProductName { get; set; }
        public string? PictureUrl { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be least one item.")]
        public int Quantity { get; set; }
        public string? Vendor { get; set; }
        public string? Category { get; set; }
    }
}