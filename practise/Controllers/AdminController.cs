using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO.Authentication;
using practise.DTO.Category;
using practise.Services.AdminServices;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public  AdminController ( IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }


        [Authorize(Roles ="Admin")]
        [HttpGet("Users")]

        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
        {
            var UsersList = await _adminServices.GetAllUsers();
            return Ok(UsersList);
        }

        [HttpGet("{userid}")]
        [Authorize(Roles ="Admin")]

        public async Task <ActionResult<GetUserDto>> GetById(Guid userid)
        {
            try
            {
                var user = await _adminServices.GetUserById(userid);
                return Ok(user);
            }
            catch(ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    
        [HttpPatch("Block&Unblock{userid}")]
        [Authorize(Roles = "Admin")]

        public async Task <IActionResult> BlockAndUnblock(Guid userid)
        {
            try
            {
              bool status =  await _adminServices.BlockAndUnblockUsers(userid);


                string message = status ? "user is blocked sucessfulley" : "User unblocked successfully";
                return Ok(new { message, IsBlocked = status });
            } catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        [HttpPost("Create-Categories")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> CreateCategorys([FromBody] AddCategoryDto addCategory)
        {
            try
            {
                var result = await _adminServices.Addcategory(addCategory);
                return Ok(new { message = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the category.", Error = ex.Message });
            }
        }
        [HttpGet("categories")]
        [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Getcategories()
            {
            try
            {
                var categories = await _adminServices.GetCategory();
                return Ok(categories);
            }catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching categories.", Error = ex.Message });
            }
            }

    }
}
