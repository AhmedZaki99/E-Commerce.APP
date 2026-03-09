namespace E_Commerce.App.Application.Abstruction.Models.Product
{
    public class ProductSpecParams
    {
        public string? Sort { get; set; }
        public int? VendorId { get; set; }
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;
        private const int PageMaxSize = 10;
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToUpper() ; }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > PageMaxSize ? PageMaxSize : value; }
        }

    }
}
