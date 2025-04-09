using System;
using System.ComponentModel.DataAnnotations;

namespace practise.DTO.Products
{
    public class ProductDtos
    {


        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Brand must be between 1 and 100 characters.")]
        public string brand { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 10,000.")]
        public int quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100,000.")]
        public int price { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public int discount { get; set; }
        public string CategoryId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters.")]
        public string description { get; set; }

        public int size { get; set; }
    }
}