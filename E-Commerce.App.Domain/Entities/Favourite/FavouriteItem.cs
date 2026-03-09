namespace E_Commerce.App.Domain.Entities.Favourite
{
    public class FavouriteItem
    {
        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public string? PictureUrl { get; set; }

        public decimal Price { get; set; }

        public string? Vendor { get; set; }

        public string? Category { get; set; }
    }
}