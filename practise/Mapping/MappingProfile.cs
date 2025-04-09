using AutoMapper;
using practise.DTO.Address;
using practise.DTO.Authentication;
using practise.DTO.Category;
using practise.DTO.Order;
using practise.DTO.Products;
using practise.Models;

namespace practise.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"));

            CreateMap<User, GetUserDto>();

            // Product mappings
            CreateMap<ProductDtos, Product>();
            CreateMap<Product, ProductGetDtos>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null));

            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<Category, AddCategoryDto>().ReverseMap();

            // Wishlist mappings
            CreateMap<WishList, WishListDto>()
                .ForMember(dest => dest.whishlistId, opt => opt.MapFrom(src => src.ProdectId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProdectId))
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Address mappings
            CreateMap<Address, AddressResDTO>().ReverseMap();
            CreateMap<Address, AddressCreateDTO>().ReverseMap();

            // Order mappings
            CreateMap<OrderItem, OrderViewDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.brand))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Product.image))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Product.price))
                .ReverseMap();
        }
    }
}