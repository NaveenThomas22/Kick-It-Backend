using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using practise.Data;
using practise.DTO.Authentication;
using practise.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace practise.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<GetUserDto>>(users);
        }

        public async Task<GetUserDto> CreateUser(RegisterDto registerDto, string role = "User")
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email.ToLower()))
            {
                throw new ArgumentException("Email already exists");
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.UserID = Guid.NewGuid();
            user.Role = role; 

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<GetUserDto>(user);
        }
        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
            {
                throw new ArgumentException("Invalid email");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid password");
            }


            var token = CreateToken(user);

            return token;
        }


        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
         new Claim (ClaimTypes.NameIdentifier, user.UserID.ToString()),
         new Claim (ClaimTypes.Email,user.Email),
         new Claim (ClaimTypes.Role, user.Role)
     };

            var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}