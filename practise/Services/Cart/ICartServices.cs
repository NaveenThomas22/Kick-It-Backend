using practise.DTO.Cart;

namespace practise.Services.Cart
{
    public interface ICartServices
    {
        public Task<bool> AddToCart(Guid userId  , Guid productId );
        public Task<CartApiResDto> GetAllCartItems(Guid Userid);

        public Task<bool> Removefromthecart(Guid userid, Guid productId );

        public Task <bool> IncreaseQuantity (Guid userid , Guid Productid );

        public Task<bool> DicreaseQuantity(Guid userid, Guid Productid);

    } 
}
