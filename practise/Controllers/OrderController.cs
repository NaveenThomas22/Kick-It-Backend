using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO.Authentication;
using practise.DTO.Order;
using practise.DTO.Payment;
using practise.Services.Order;
using practise.Services.RazorPay;
using System.Security.Claims;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _services;
        private readonly IRazorPayService _razorPayService;

        public OrderController(IOrderServices services,IRazorPayService razorPayService)
        {
            _services = services;
            _razorPayService = razorPayService;
        }

        [HttpPost("create_razorpay")]
        [Authorize]

        public async Task<IActionResult> createRazorPayOrder(int  price)
        {
            var result = await _razorPayService.createRazorpayment(price);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Verify-Payment")]
        [Authorize]

        public async Task<IActionResult> VerifyPayment(PaymentDto payment)
        {
            var result = await _razorPayService.RazorPayment(payment);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("PlaceOrder")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrder)
        {
            try
            {
         

                var userId = Guid.Parse(HttpContext.Items["userid"].ToString());
                if (userId == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

    

                var orderResult = await _services.CreateOrder(userId, createOrder);

                if (orderResult)
                {
                    return Ok(new { message = "Order placed successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Cannot place the order" });
                }
            }
            catch (Exception ex)
            {
     
                return StatusCode(500, new { message = "An error occurred while placing the order: " + ex.Message });
            }
        }

        [HttpGet("User-Retrival")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> GetAllOrders()
        {
            try
            {


            var userId = Guid.Parse(HttpContext.Items["userid"].ToString());
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }
            var res = await _services.GetOrderDetails(userId);
                if (res != null) {
                    return Ok(res);
                }else
                {
                    return BadRequest(new { message = "error occured while seeing the product" });
                }
            } catch (Exception ex)
            {
     
                return StatusCode(500, new { message = "An error occurred while placing the order: " + ex.Message });
            }
        }

        
        [HttpGet("{userid}")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> GetOrderById(Guid userid)
        {
            try
            {
          
            var res = await _services.GetOrderById(userid);
                if (res != null) {
                    return Ok(res);
                }else
                {
                    return BadRequest(new { message = "error occured while seeing the product" });
                }
            } catch (Exception ex)
            {
     
                return StatusCode(500, new { message = "An error occurred while placing the order: " + ex.Message });
            }

        }
        [HttpGet("revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenue()
        {
            try
            {
                var revenue = await _services.GetRevenue();
                return Ok(revenue);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving revenue.");
            }
        }
        
        [HttpPut("change-status/{orderId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ChangeStatus(Guid orderId, [FromBody] string status)
        {
            try
            {
                var result = await _services.ChangeStatus(orderId, status);

                if (result.Message == "invalid status")
                {
                    return BadRequest(result); 
                }

                return Ok(result); 
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine($"Error: {ex.Message}");

                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        
    }
}