namespace practise.DTO.Cart
{
    public class CartApiResDto
    {
        public decimal TotalPrice { get; set; }

       public int TotalCount { get; set; }

       public List <CartResDto> CartProducts { get; set; }
    }
}
