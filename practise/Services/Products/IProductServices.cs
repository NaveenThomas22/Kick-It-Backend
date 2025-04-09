using practise.DTO.Products;
using practise.Models;

namespace practise.Services.Products
{
    public interface IProductServices
    {
        Task  AddProduct(ProductDtos productDtos, IFormFile image);

        Task <List<ProductGetDtos>> GetAllProducts();

        Task<ProductGetDtos> GetProductById(Guid productid);
        Task <List<ProductGetDtos>>  GetAllProducts(int PageNumber = 1, int PageSize = 10);

        Task<bool> UpdateProduct(Guid productid, ProductDtos productdtos , IFormFile image);

        Task<bool> DeleteProduct(Guid id);
    }
}
