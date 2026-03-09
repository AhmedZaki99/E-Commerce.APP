namespace E_Commerce.App.Domain.Entities.Favourite
{
    public class Favourites
    {
        public required string Id { get; set; } // userId

        public IEnumerable<FavouriteItem> Items { get; set; } = new List<FavouriteItem>();
    }
}
