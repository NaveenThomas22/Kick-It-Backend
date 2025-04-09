using AutoMapper;
using Microsoft.EntityFrameworkCore;
using practise.Data;
using practise.DTO.Authentication;
using practise.DTO.Category;
using practise.Models;

namespace practise.Services.AdminServices
{
    public class AdminSerives : IAdminServices

    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminSerives ( AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<GetUserDto>>(users);

        }

        public async Task<GetUserDto> GetUserById(Guid userid)
        {
            var userbyid = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userid);
            if(userid == null)
            {
                throw new InvalidOperationException("user not found");
            }
            return _mapper.Map<GetUserDto>(userbyid);
        }

        public async Task <bool> BlockAndUnblockUsers(Guid userid)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userid);
             if(foundUser == null)
            {
                throw new ArgumentException("user  not found ");
            }
             if (foundUser.IsBlocked)
            {
            foundUser.IsBlocked = false;
            }
            else
            {
                foundUser.IsBlocked =true;

               
            }
            await _context.SaveChangesAsync();
            return foundUser.IsBlocked;

        }

        public async Task<string> Addcategory(AddCategoryDto addCategory)
        {
            try
            {
                if (addCategory == null || string.IsNullOrEmpty(addCategory.CategoryName))
            {
                throw new ArgumentException("category data is invalid ");
            }

            var category = _mapper.Map<Category>(addCategory);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return " category added successfully";
            }catch (Exception ex)
            {
                throw new Exception("error adding category : " + ex.Message);

            }
        
    }

        public async Task<IEnumerable<GetCategoryDto>> GetCategory()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching categories: " + ex.Message);
            }

        }
    }

}