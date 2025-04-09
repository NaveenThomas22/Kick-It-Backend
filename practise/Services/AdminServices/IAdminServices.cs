using practise.DTO.Authentication;
using practise.DTO.Category;

namespace practise.Services.AdminServices
{
    public interface IAdminServices
    {

        Task<IEnumerable<GetUserDto>> GetAllUsers();
        Task<GetUserDto> GetUserById(Guid userid);

        Task <bool> BlockAndUnblockUsers(Guid userid);

        Task<IEnumerable<GetCategoryDto>> GetCategory();

        Task<string> Addcategory(AddCategoryDto addCategory);




    }


}
