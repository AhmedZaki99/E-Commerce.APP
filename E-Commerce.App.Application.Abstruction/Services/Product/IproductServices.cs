using E_Commerce.App.Application.Abstruction.Common;
using E_Commerce.App.Application.Abstruction.Models.Product;

namespace E_Commerce.App.Application.Abstruction.Services.Product
{
    public interface IproductServices
    {
        public Task<Pagination<ProductToReturnDto>> GetAllProductAsync(ProductSpecParams specParams);
        public Task<ProductToReturnDto> GetProduct(int id);
        public Task<IEnumerable<VendorDto>> GetAllBrandAsync();
        public Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
    }
}
