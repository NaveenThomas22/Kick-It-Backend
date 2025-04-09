using System.ComponentModel.DataAnnotations;

namespace practise.Models
{
    public class  CartItems
    {
        public Guid id { get; set; }

        [Required]
        public Guid cartId { get; set; }

        public  Cart Cart { get; set; }

        [Required]  
        public Guid ProductId { get; set; }
 
        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int  Quatity { get; set; }

    }
}
