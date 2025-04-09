namespace practise.DTO.Products
{
    public class WishListDto
    {
        public Guid whishlistId { get; set; }
        public Guid ProductId { get; set; }
        
        public string Brand { get; set; }

        public int Price { get; set; }

        public string Image { get; set; }
            
    }
}
