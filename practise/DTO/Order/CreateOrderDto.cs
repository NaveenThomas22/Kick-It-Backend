namespace practise.DTO.Order
{
    public class CreateOrderDto
    {
        public Guid AddressId { get; set; }
        public int TotalAmount { get; set; }
        public string TransactionId { get; set; }

    }
}