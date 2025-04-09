using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using practise.DTO.Products;
using practise.Services.Products;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace practise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost("addProducts")]
        [Authorize(Roles ="Admin")]

        
        public async Task <IActionResult> AddProduct([FromForm] ProductDtos productdto, IFormFile image)
        {
            try
            {
                await _productServices.AddProduct(productdto, image);
                return Ok("product addeed succesfully");
            }catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productServices.GetProductById(id);
            if (product == null)
            {
                return NotFound(new { message = "product is not added" });
            }
            return Ok(product);
        }

            [HttpGet("GetAllProducts")]
            [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
            var products = await _productServices.GetAllProducts(pageNumber, pageSize);
            
                if (products == null)
                {
                    throw new ArgumentException("The product is null");
                }
                else
                {
                    return Ok(products);
                }

            }catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct( Guid id, [FromForm] ProductDtos productDto, IFormFile image)
        {
            try
            {
                if (productDto == null)
                {
                    return BadRequest(new { message = "Product data is required." });
                }

                var updatedProduct = await _productServices.UpdateProduct(id, productDto, image);
                
                 return Ok(new { Message = "Product updated succesfully" });
              
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {

            var result = await _productServices.DeleteProduct(id);
            if (!result)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(new { message = "Product deleted successfully." });
            } catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [HttpGet("Get/All/Products")]


        public async Task<IActionResult> GetAllProdutList()
        {
            try
            {
                var TotalProuducts = await _productServices.GetAllProducts();
                return Ok(TotalProuducts);
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("error occure while getting the products", Ex.Message);
            }

        }

    }
}
