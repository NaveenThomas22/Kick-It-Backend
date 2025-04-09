namespace practise.DTO.Products
{
    public class ProductGetDtos
    {
        public Guid id { get; set; }
        public string brand { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public string image { get; set; }
        public int discount { get; set; }
        public string CategoryName { get; set; }
        public string description { get; set; }
        public int size { get; set; }
    }
}
