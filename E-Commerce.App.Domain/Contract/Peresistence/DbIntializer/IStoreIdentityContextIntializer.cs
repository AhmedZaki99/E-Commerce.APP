namespace E_Commerce.App.Domain.Contract.Peresistence.DbIntializer
{
    public interface IStoreIdentityContextIntializer
    {
        Task UpdateDateBase();
        Task SeedData();
    }
}
