using practise.DTO.Payment;
using practise.Models;

namespace practise.Services.RazorPay
{
    public interface IRazorPayService
    {
        Task <Responses<string>> createRazorpayment(int price);
        Task<Responses<bool>> RazorPayment(PaymentDto payment);


    }
}
