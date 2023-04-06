using Foody.DTOs;
using Foody.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpDelete("delete-user/{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var result = await _userService.DeleteUser(Id);
            return Ok(result);
        }

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var result = await _userService.GetUserByEmail(email);
            return Ok(result);
        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> GetUserByEmailAsync(UpdateUserDto model)
        {
            var result = await _userService.UpdateUser(model);
            return Ok(result);
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UploadPhoto(IFormFile file, string id)
        {
            var result = await _userService.UploadPhoto(file, id);
            return Ok(result);
        }

        [HttpPost("get-all-users")]
        public IActionResult GetAllUsers(PagingParameter param)
        {
            var result = _userService.GetAllUser(param);
            return Ok(result);
        }
    }
}
