using E_Commerce.App.Domain.Entities.Product;

namespace E_Commerce.App.Domain.Specifications
{
    public class ProductWithFilterationForCountPagination : BaseSpecifications<Product , int>
    {
        public ProductWithFilterationForCountPagination( int? VendorId, int? CategoryId, string? Search)
            : base(p =>
            (string.IsNullOrEmpty(Search) || p.NormalizedName.Contains(Search))
                  &&
            (!VendorId.HasValue || p.VendorId == VendorId.Value) &&
                        (!CategoryId.HasValue || p.CategoryId == CategoryId.Value)
                  )
        {
            
        }
    }
}
