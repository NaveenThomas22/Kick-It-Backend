using practise.DTO.Address;

namespace practise.DTO.Order
{
    public class ViewUserOrderDetailsDto
    {

        public DateTime OrderDate { get; set; }
        public Guid OrderId { get; set; }
        public int TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public AddressCreateDTO Address { get; set; }

        public string TransactionId { get; set; }

        public List<OrderViewDto> OrderProduct { get; set; }
    }
}
