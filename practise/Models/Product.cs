using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace practise.Models
{
    public class Product
    {
        public Guid id { get; set; }
        public string brand { get; set; } 
        public int quantity { get; set; }
        public int price { get; set; } 
        public int discount { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int size { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual List<CartItems> CartItem { get; set; }

    }
}
