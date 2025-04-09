using Microsoft.EntityFrameworkCore;
using practise.Data;
using practise.DTO.Cart;
using practise.Models;
using System;
using System.Linq.Expressions;

namespace practise.Services.Cart
{
    public class CartServices

        : ICartServices
    {
        private readonly AppDbContext _context;

        public CartServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToCart(Guid userId, Guid productId)
        {
            try
            {
                if (userId == null) throw new Exception("user not valid");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserID == userId);


                var product = await _context.Products.FirstOrDefaultAsync(a => a.id == productId);

                if (product == null) throw new Exception("product with this id not found");

                if (user.Cart == null)
                {
                    user.Cart = new practise.Models.Cart
                    {
                        userId = userId,
                        cartItems = new List<CartItems>()

                    };

                    _context.Cart.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }

                CartItems existingProducts = user.Cart.cartItems.FirstOrDefault(p => p.ProductId == productId);

                if (existingProducts == null)
                {
                    CartItems cartItem = new CartItems
                    {
                        cartId = user.Cart.CartId,
                        ProductId = productId,
                        Quatity = 1

                    };
                    _context.cartItems.Add(cartItem);
                }
                else
                {
                    if (existingProducts.Quatity < 10)
                    {
                        existingProducts.Quatity++;
                    }
                }

                await _context.SaveChangesAsync();

                return true;



            } catch (Exception ex)
            {
                Console.WriteLine("error occur while adding the product to the cart ", ex.Message);
                return false;
            }
        }

        public async Task<bool> DicreaseQuantity(Guid userid, Guid Productid)
        {

            try
            {

                if (userid == Guid.Empty)
                {
                    throw new ArgumentException("No user found ");
                }

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserID == userid);

                if (user.Cart.cartItems.Count == 0)
                {
                    throw new ArgumentException("Your cart is empty");
                }
                else
                {
                    var cartitem = user.Cart.cartItems.FirstOrDefault(ci => ci.ProductId == Productid);
                    if (cartitem.Quatity <= 0)
                    {
                        throw new Exception(" minimum quantity is 0");
                    }
                    else
                    {
                        cartitem.Quatity--;
                        await _context.SaveChangesAsync();
                        return true;
                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured while decreasing  the quantity of the product  " + ex.Message);
            }


        }

        public async Task<CartApiResDto> GetAllCartItems(Guid Userid)
        {

            try
            {
                if (Userid == Guid.Empty)
                {
                    throw new ArgumentException("Login to see the cart");
                }

                var cart = await _context.Cart
                    .Include(c => c.cartItems)
                    .ThenInclude(u => u.Product)
                    .FirstOrDefaultAsync(u => u.userId == Userid);

                if (cart?.cartItems == null)
                {
                    throw new Exception("the cart is empty ");
                }
                if (cart != null)
                {
                    var cartItems = cart.cartItems.Select(u => new CartResDto
                    {
                        ProductId = u.ProductId,
                        Brand = u.Product.brand,
                        Price = u.Product.price,
                        Image = u.Product.image,
                        Qunatity = u.Quatity,
                        TotalPrice = u.Product.price * u.Quatity,

                    }).ToList();

                    var totalcount = cartItems.Count();
                    var totalPrice = cartItems.Sum(u => u.TotalPrice);
                    return new CartApiResDto
                    {
                        TotalCount = totalcount,
                        TotalPrice = totalPrice,
                        CartProducts = cartItems
                    };
                }

                return new CartApiResDto();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



        }
        public async Task<bool> IncreaseQuantity(Guid userid, Guid Productid)
        {
            try
            {
                if (userid == Guid.Empty)
                {
                    throw new ArgumentException("No user found ");
                }

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.cartItems)
                    .FirstOrDefaultAsync(u => u.UserID == userid);

                if (user.Cart.cartItems.Count == 0)
                {
                    throw new ArgumentException("Your cart is empty");
                }
                else
                {
                    var cartitem = user.Cart.cartItems.FirstOrDefault(ci => ci.ProductId == Productid);
                    if (cartitem.Quatity == 10) // Error: cartitem might be null
                    {
                        throw new Exception(" maximum quantity 10");
                    }
                    else
                    {
                        cartitem.Quatity++;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred while increasing the quantity of the product " + ex.Message);
            }
        }


        public async Task<bool> Removefromthecart(Guid userid, Guid productId)
        {
            try
            {

                if (userid == Guid.Empty)
                {


                    throw new ArgumentException("userid not found");
                }


                var user = await _context.Users
                     .Include(u => u.Cart)
                     .ThenInclude(c => c.cartItems)
                     .FirstOrDefaultAsync(u => u.UserID == userid);

                var product = await _context.Products.FirstOrDefaultAsync(u => u.id == productId);

                if (user != null && product != null)
                {
                    var item = user.Cart.cartItems.FirstOrDefault(u => u.ProductId == productId);
                           if (item != null)
                    {
                        _context.cartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }

            return false;

                throw new Exception("exception occure while deleting a product from cart");
            }
         catch(Exception ex)

               {
            return false;

            throw new Exception("exception occure while deleting a product from cart  " + ex.Message);
            }

        }
    }


}