using practise.DTO.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace practise.Services.UserServices
{
    public interface IUserService
    {

        Task<GetUserDto> CreateUser(RegisterDto registerDto, string role = "User");
        Task<string> Login(LoginDto loginDto);
    }
}