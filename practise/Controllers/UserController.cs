using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practise.DTO.Authentication;
using practise.Services.UserServices;


namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<GetUserDto>> Create(RegisterDto registerDto, [FromQuery] string role = "User")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                var createdUser = await _userService.CreateUser(registerDto, role);
                return Ok(createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                var token = await _userService.Login(loginDto);
                return Ok(new { token });
            }
            catch (ArgumentException ex) when (ex.Message == "Invalid email")
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex) when (ex.Message == "Invalid password")
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}