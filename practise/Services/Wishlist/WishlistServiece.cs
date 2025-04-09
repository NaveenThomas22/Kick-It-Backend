using AutoMapper;
using Microsoft.EntityFrameworkCore;
using practise.Data;
using practise.DTO.Products;
using practise.Models;
using System.Linq.Expressions;

namespace practise.Services.Wishlist
{
    public class WishlistServiece : IWishlistService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
             

        public WishlistServiece(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async  Task<bool> AddToWishList(Guid userid, Guid productid)
        {

            try
            {
                var isthereProduct = await _context.Products.FirstAsync(p => productid == productid);

                if(isthereProduct == null)
                {
                    throw new ArgumentNullException(" product not found ");
                }

                var existingWishlist = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.ProdectId == productid && w.UserID == userid );
                if (existingWishlist != null)
                {
                    throw new ArgumentException("product already exists");
                    return false;
                }

                var wishlist = new WishList()
                {
                    WishlistId = Guid.NewGuid(),
                    ProdectId = productid,
                    UserID = userid
                };
                await _context.WishLists.AddAsync(wishlist);
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        public async Task<bool> DeleteProduct(Guid wid)
        {
            try
            {

            var wishlistProduct = await _context.WishLists.FirstOrDefaultAsync(w => w.WishlistId == wid);
            if (wishlistProduct == null)
            {
                return false;
            }
            _context.WishLists.Remove(wishlistProduct);
            await _context.SaveChangesAsync();
            return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
     
        }

        public async Task<List<WishListDto>> ViewwishlistData(Guid id)
        {
            try
            {
                var wishlistItems = await _context.WishLists
                    .Where(w => w.UserID == id)
                    .Join(
                        _context.Products,
                        wishlist => wishlist.ProdectId,
                        product => product.id,
                        (wishlist, product) => new WishListDto
                        {
                            whishlistId = wishlist.WishlistId,
                            ProductId = wishlist.ProdectId,
                            Brand = product.brand,
                            Price = product.price,
                            Image = product.image
                        })
                    .ToListAsync();

                return wishlistItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewWishlistData: {ex.Message}");
                throw;
            }
        } 

        
     
    }
}
