using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practise.Services.Wishlist;
using System.Diagnostics.CodeAnalysis;

namespace practise.Controllers
{
    public class WishListController : Controller
    {
        private readonly IWishlistService _wishlistService;
        public WishListController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("AddToWishList")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> ViewWishlist(Guid productid)
        {
            try
            {

            var userid = Guid.Parse(HttpContext.Items["userid"].ToString());

            var res = await _wishlistService.AddToWishList(userid,productid);

            if (!res)
            {
                return NotFound("invalid productId");
            }
            return Ok("sussefully added the product ");
            }catch (Exception ex)
            {
                return (StatusCode(500, ex.Message));
            }

            

        }

        [HttpGet ("view wishlist")]
        [Authorize(Roles ="User")]

        public async Task<IActionResult> ViewWishList()
        {
            try
            {
                var userid = Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _wishlistService.ViewwishlistData(userid);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteWishList")]
        [Authorize(Roles ="Users")]

        public async Task <IActionResult> DelteWishlistData(Guid wid)
        {
            try
            {
                var res = await _wishlistService.DeleteProduct(wid);
                if (!res)
                {
                    return NotFound("Invalid wishlist id ");
                }
                return Ok("successfully removed");
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
