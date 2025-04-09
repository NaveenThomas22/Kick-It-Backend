namespace practise.DTO.Cart
{
    public class CartResDto
    {
        public Guid ProductId { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }

        public string Image { get; set; }
        public decimal TotalPrice { get; set; }
        public int Qunatity { get; set; }

     }
}
