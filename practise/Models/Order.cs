namespace practise.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
         

        public Guid  userId { get; set; }
        public Guid  AddressId { get; set; }
        public int TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public string TransactionId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
    }
}

