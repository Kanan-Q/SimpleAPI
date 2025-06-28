using SimpleAPI.Core.Entities.Common;

namespace SimpleAPI.Core.Entities
{
    public sealed class Information :BaseEntity
    {
        public string ProductName { get; set; }
        public string Description{ get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
