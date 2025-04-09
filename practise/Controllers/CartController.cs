using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.Models;
using practise.Services.Cart;
using System.Diagnostics.CodeAnalysis;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices  _cartServieces;
            public CartController (ICartServices cartServices)
        {
            _cartServieces = cartServices;
        }

        [HttpPost("AddToCart")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult > AddCart (Guid productid)
        {
            try
            {
                var  userid = Guid.Parse(HttpContext.Items["userid"].ToString());
                var res = await _cartServieces.AddToCart(userid, productid);
                if( res == true)
                {
                    return Ok();    
                }
                return BadRequest (new { message = "Failed to add to cart." });
            }
            catch (Exception ex)
            {
                return BadRequest("invalid user id format");
            }
        }

        [HttpGet("GetcartItems")]   
        [Authorize(Roles ="User")]
        public async Task <IActionResult> GetCartItems ()
       {
            try
            {
                var userid = Guid.Parse(HttpContext.Items["userid"].ToString());
            var cartitems = await _cartServieces.GetAllCartItems(userid);
            return Ok(cartitems);
            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }

        [HttpDelete("RemoveCartItem")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> RemoveCartItem ( Guid productid)
        {
            try
            {
                var userId= Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _cartServieces.Removefromthecart(userId, productid);
                if (res == true)
                {
                    return Ok(new {message = "cart item removed successfully"});
                }
                return BadRequest(new {message = "failed to remove the cart item"});

            }catch (Exception ex)
            {
                return BadRequest(new { message = " an error occured " + ex.Message });
                    
            }
        }
        [HttpPut("increase")]
        [Authorize(Roles ="User")]
        public async Task <IActionResult> IncreaseQunatity (Guid productid)
        {
            try
            {

              var userid = Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _cartServieces.IncreaseQuantity(userid, productid);
                if (res == true)
                {
                    return Ok(new {message = "sucessfully incremented the quantity"});              
                }
                else
                {
                    return BadRequest(new { mesagae = " failed to increment the quantity" });
                }
            }catch (Exception ex)
            {
                return BadRequest(new { message = " failed to increment the quantity " + ex.Message });
            }
        }

        [HttpPut("Decrease")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DicreaseQuantityItem(Guid productid)
        {
            try
            {

                var userid = Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _cartServieces.DicreaseQuantity(userid, productid);
                if (res == true)
                {
                    return Ok(new { message = "sucessfully Decremented  the quantity" });
                }
                else
                {
                    return BadRequest(new { mesagae = " failed to decrement the quantity" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = " failed to decrement  the quantity " + ex.Message });
            }
        }


    }
}
