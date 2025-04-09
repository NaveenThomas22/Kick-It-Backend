using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO.Address;
using practise.Services.Address;
using System.Runtime.CompilerServices;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressServices _addressServices;

        public AddressController (IAddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        [HttpPost("AddAddress")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> CreateAddress ([FromBody] AddressCreateDTO newAddress)
        {
            try
            {

            var UserId = Guid.Parse(HttpContext.Items["userid"].ToString());
                var res = await _addressServices.CreateAddress(newAddress, UserId);

                if (res == true)
                {
                    return Ok(new { message = "Address sucessfully created" });
                }
                else
                {
                    return BadRequest(new { message = " Failed to complete address" });
                }
            }catch (Exception ex)
            {
                return BadRequest(new { message = "an error occured :" + ex.Message });
            }

           

        }
        [HttpGet("getAddress")]
        [Authorize(Roles = "User")]

        public async Task <IActionResult> GetAddress()
        {
            try
            {

            var userid = Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _addressServices.GetAllAddress(userid);

                if (res != null)
                {
                    return Ok(res);
                }else
                {
                    return BadRequest(new { message = "Fail to load the address" });
                }

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpDelete("RemoveAddress")]
        [Authorize(Roles ="User")]
        public async Task <IActionResult> RemoverAddress (Guid AddressId)
        {
            try
            {
 
            var userId = Guid.Parse(HttpContext.Items["userid"].ToString());

                var res = await _addressServices.DeleteAddress(userId, AddressId);
                if (res ==  true)
                {
                    return Ok(new { message ="sucessfully deleted the product"});
                }
                else
                {
                    return BadRequest(new { message = "Failed to delete the product" });
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
