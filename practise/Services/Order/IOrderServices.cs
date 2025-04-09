using Azure;
using practise.DTO.Order;

namespace practise.Services.Order
{
    public interface IOrderServices
    {
        public Task<bool> CreateOrder (Guid userid, CreateOrderDto createOrderDtos);

        public Task<List<ViewUserOrderDetailsDto>> GetOrderDetails(Guid Userid);

        public Task<List<ViewUserOrderDetailsDto>> GetOrderById(Guid userid);

        public Task<TotalRevenueDto> GetRevenue();

        public Task<AddStatusDto> ChangeStatus(Guid orderId, string status);
    }
}
