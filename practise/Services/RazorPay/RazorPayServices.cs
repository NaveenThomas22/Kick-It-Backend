using practise.Data;
using practise.DTO.Payment;
using practise.Models;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace practise.Services.RazorPay
{
    public class RazorPayServices : IRazorPayService
    {
        private readonly string _razorpayKey;
        private readonly string _razorpaySecret;
        private readonly AppDbContext _context;

        public RazorPayServices(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _razorpayKey = configuration["Razorpay:Key"];
            _razorpaySecret = configuration["Razorpay:Secret"];
        }

        public async Task<Responses<string>> createRazorpayment(int price)
        {
            try
            {
                if (price <= 0)
                {
                    return new Responses<string> { StatusCode = 400, Message = "enter a valid price" };
                }

                Dictionary<string, object> input = new Dictionary<string, object>
                {
                    {"amount", price * 100},
                    {"currency", "INR"}
                };

                RazorpayClient client = new RazorpayClient(_razorpayKey, _razorpaySecret);
                Razorpay.Api.Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();

                return new Responses<string>
                {
                    StatusCode = 200,
                    Message = "OrderId created successfully",
                    Data = orderId
                };
            }
            catch (Exception ex)
            {
                return new Responses<string>
                {
                    StatusCode = 500,
                    Message = "error creating razorpay order: " + ex.Message
                };
            }
        }

        public async Task<Responses<bool>> RazorPayment(PaymentDto payment)
        {
            if (payment == null ||
                string.IsNullOrEmpty(payment.razorpay_payment_id) ||
                string.IsNullOrEmpty(payment.razorpay_order_id) ||
                string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return await Task.FromResult(new Responses<bool> { StatusCode = 400, Message = "payment credentials not found" });
            }

            try
            {
                string generatedSignature = GenerateSignature(payment.razorpay_payment_id, payment.razorpay_order_id, _razorpaySecret);
                if (generatedSignature == payment.razorpay_signature)
                {
                    // Optional: Update database to mark payment as successful
                    // Example: _context.Orders.Where(o => o.OrderId == payment.razorpay_order_id).FirstOrDefault().Status = "Paid";
                    // await _context.SaveChangesAsync();

                    return await Task.FromResult(new Responses<bool> { StatusCode = 200, Message = "Payment Successful", Data = true });
                }
                else
                {
                    return await Task.FromResult(new Responses<bool> { StatusCode = 400, Message = "invalid signature verification failed" });
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Responses<bool> { StatusCode = 500, Message = "error while verifying the razorpayment: " + ex.Message });
            }
        }

        private string GenerateSignature(string paymentId, string orderId, string secret)
        {
            string stringToSign = orderId + "|" + paymentId;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}