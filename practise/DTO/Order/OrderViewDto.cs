namespace practise.DTO.Order
{
    public class OrderViewDto
    {
        public Guid ProductId { get; set; }
        public Guid orderItemId { get; set; }
        public string ProductName { get; set; }
        public string Url { get; set; }
        public int price { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
    }
}
