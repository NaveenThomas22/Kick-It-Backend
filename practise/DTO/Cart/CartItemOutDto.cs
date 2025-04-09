namespace practise.DTO.Cart
{
    public class CartItemOutDto
    {
        public Guid ProductId { get;set; }

        public string  ProductBrand { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get;set; }

        public string Image { get; set; }
    }
}
