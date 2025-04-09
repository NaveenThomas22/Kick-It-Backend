using practise.DTO.Products;

namespace practise.Services.Wishlist
{
    public interface IWishlistService
    {
        public Task <bool> AddToWishList(Guid userid, Guid productid);
        Task<List<WishListDto>> ViewwishlistData(Guid id);

        Task<bool> DeleteProduct(Guid id);
         
      
    }
}
