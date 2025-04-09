using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using practise.Data;
using practise.DTO.Address;
using practise.DTO.Order;
using practise.Models;
using System.Security.Cryptography.Xml;

namespace practise.Services.Order
{
    public class OrderServices : IOrderServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderServices(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<AddStatusDto> ChangeStatus(Guid orderId, string status)
        {
            try
            {
                string[] validstatueses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled", "Returned" };
               if (!validstatueses.Contains(status))
                {
                    return new AddStatusDto { Message = "invalidstatus" };
                }
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return new AddStatusDto { Message = " order not found" };
                }
                order.OrderStatus = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return new AddStatusDto {
                    OrderStatus = status,
                    Message = " order status updated sucessfully"
                };


            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in ChangeStatus: {ex.Message}");
                return new AddStatusDto { Message = "An error occurred while updating the order status" };
            }
        }

        public async Task<bool> CreateOrder(Guid userid, CreateOrderDto createOrderDtos)
        {
            try
            {
                var addresss = await _context.Address.FirstOrDefaultAsync(s => s.AddressId == createOrderDtos.AddressId && s.Userid == userid);

                if (addresss == null)
                {
                    throw new ArgumentException("Cant get the address");
                }
                var cart = await _context.Cart
                   .Include(c => c.cartItems)
                   .ThenInclude(u => u.Product)
                   .FirstOrDefaultAsync(c => c.userId == userid);

                if (cart == null)
                {
                    throw new ArgumentException(" your car  is empty");
                }

                var order = new practise.Models.Order
                {
                    userId = userid,
                    OrderId = Guid.NewGuid(),
                    OrderTime = DateTime.Now,
                    AddressId = createOrderDtos.AddressId,
                    TotalPrice = createOrderDtos.TotalAmount,
                    OrderStatus = "Pending",
                    TransactionId = createOrderDtos.TransactionId,
                    OrderItems = cart.cartItems.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quatity,
                        TotalPrice = c.Quatity * c.Product.price,
                    }).ToList()

                };



                foreach (var CartItem in cart.cartItems)
                {
                    var product = CartItem.Product;
                    if (product.quantity < CartItem.Quatity)
                    {
                        throw new ArgumentException(" out of stock ");
                    }
                    ;
                    product.quantity -= CartItem.Quatity;
                   
                    _context.Products.Update(product);
                    
                }

                await _context.Orders.AddAsync(order);
                _context.cartItems.RemoveRange(cart.cartItems);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ViewUserOrderDetailsDto>> GetOrderById(Guid userid)
        {
            try
            {
                if (userid == Guid.Empty)
                {
                    throw new ArgumentException("cant find the user ");
                }
                var orders = await _context.Orders
                    .Include(u => u.Address)
                    .Include(u => u.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(u => u.userId == userid)
                   .ToListAsync();


                if (orders == null)
                {
                    throw new ArgumentNullException("No Order found ");
                }
                var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto
                {
               
                    OrderId = o.OrderId,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderTime,
                    OrderStatus = o.OrderStatus,
                    TransactionId = o.TransactionId,
                    Address = _mapper.Map<AddressCreateDTO>(o.Address),
                    OrderProduct = _mapper.Map<List<OrderViewDto>>(o.OrderItems)
                }).ToList();
                return orderDetails;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public async Task<List<ViewUserOrderDetailsDto>> GetOrderDetails(Guid Userid)
        {
            try
            {
                if (Userid == Guid.Empty)
                {
                    throw new ArgumentException("cant find the user ");
                }
                var orders = await _context.Orders
                   .Include(u => u.Address)
                   .Include(u => u.OrderItems)
                   .ThenInclude(oi => oi.Product)
                   .Where(u => u.userId == Userid)
                   .ToListAsync();

                if (orders == null)
                {
                    throw new ArgumentNullException("No Order found ");
                }
                var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto
                {
               
                    OrderId = o.OrderId,
                    TotalPrice = o.TotalPrice,
                    OrderDate = o.OrderTime,
                    OrderStatus = o.OrderStatus,
                    TransactionId = o.TransactionId,
                    Address = _mapper.Map<AddressCreateDTO>(o.Address), 
                    OrderProduct = _mapper.Map<List<OrderViewDto>>(o.OrderItems) 
                }).ToList();
                return orderDetails;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public async Task<TotalRevenueDto> GetRevenue()
        {
            try
            {
                var OrderItems = await _context.OrderItems.Include(oi => oi.Product).ToListAsync();
                var amount = OrderItems.Sum(oi => oi.TotalPrice);
                var items = OrderItems.Sum(oi => oi.Quantity);

                return new TotalRevenueDto { TotalRevenue = amount, TotalItemsSold = items };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRevenue: {ex.Message}");
                throw;
            }
        }
    }
}
