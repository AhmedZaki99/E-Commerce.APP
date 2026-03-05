#nullable disable
namespace E_Commerce.App.Domain.Common
{
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? LastModifideOn { get; set; }
    }
}
