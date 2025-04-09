using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using practise.Data;
using practise.DTO.Products;
using practise.Models;
using practise.Services.CloudinaryServices;
using System.Security.Cryptography.Xml;

namespace practise.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;


        public ProductServices(AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
         
        }

        public async Task AddProduct(ProductDtos productDtos ,IFormFile image)
        {
            try
            {
                string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                var product = _mapper.Map<Product> (productDtos);
                product.image = imageUrl;

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();

            }catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
           
        }

        public async Task<ProductGetDtos> GetProductById(Guid productid)
        {
            try
            {

            var product = await _context.Products
                .Include(c => c.Category)
                .FirstOrDefaultAsync(u => u.id == productid);        
            if (product == null)
            {
                throw new ArgumentException("Product ID not found");
            }
            return _mapper.Map<ProductGetDtos> (product);
            }catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
        }

        public async Task<List<ProductGetDtos>> GetAllProducts (int PageNumber, int PageSize)
        {
          try
            {
                var products = await _context.Products
                    .Include(x => x.Category)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                return _mapper.Map<List<ProductGetDtos>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateProduct(Guid id, ProductDtos productDtos,IFormFile image)
        {
            try
            {

            if (productDtos == null)
            {
                throw new ArgumentNullException("No product details exist");
            }

            var productExisting = await _context.Products.FirstOrDefaultAsync(u => u.id == id);
            if (productExisting == null)
            {
                throw new ArgumentException("Product not found");
            }

            string imageUrl = await _cloudinaryService.UploadImageAsync(image);


            productExisting.image=imageUrl;
            _mapper.Map(productDtos,productExisting);


            _context.Products.Update(productExisting);
            await _context.SaveChangesAsync();

            return true;
            }catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            try
            { 
                if(id == Guid.Empty)
                {
                    throw new ArgumentException("Product cannot be null");
                 
                }

            var product = await _context.Products.FirstOrDefaultAsync(u => u.id == id);

                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {id} not found.");
                }
        
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
            }catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

        }

        public async Task <List<ProductGetDtos>> GetAllProducts()
        {
            try
            {

                var product = await _context.Products
                    .Include(x => x.Category)
                    .ToListAsync();

                return _mapper.Map <List<ProductGetDtos>>(product);
                    
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
    