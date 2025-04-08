using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound(new { message = "No users found." });
            }

            return Ok(users);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var createdUser = await userService.AddUserAsync(UserDto.MapUserDtoToUser(userDto));
            if (createdUser == null)
            {
                return BadRequest(new { message = "Failed to create user." });
            }

            return Ok(createdUser);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            var updatedUser = await userService.UpdateUserAsync(id, UserDto.MapUserDtoToUser(userDto));
            if (updatedUser == null)
            {
                return BadRequest(new { message = "Failed to update user." });
            }

            return Ok(updatedUser);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            await userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
