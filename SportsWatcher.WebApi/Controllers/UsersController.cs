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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await userService.GetAllUsersAsync();
            if( users == null )
            {
                return NotFound(new { message = "No users found." });
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var createdUser = await userService.AddUserAsync(user);
            if (createdUser == null)
            {
                return BadRequest(new { message = "Failed to create user." });
            }

            return Ok(createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.UserId != id)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            var updatedUser = await userService.UpadeUserAync(user);
            if (updatedUser == null)
            {
                return BadRequest(new { message = "Failed to update user." });
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            await userService.DeleteUserAsync(id);
            if (existingUser == null)
            {
                return BadRequest(new { message = "Failed to delete user." });
            }

            return NoContent();
        }
    }
}
