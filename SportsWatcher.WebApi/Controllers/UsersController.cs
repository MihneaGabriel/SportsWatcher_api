using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.DTOs;
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
            if (users == null || users.Count() == 0)
            {
                return NotFound(new { message = "No users found." });
            }

            return Ok(users);
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] LoginDto login)
        {
            var userDto = await userService.GetUserAsync(login);
            if (userDto == null)
            {
                return NotFound(new { message = "Email or password is incorrect." });
            }

            return Ok(userDto);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            if( userDto.AgeConfirmation == false)
            {
                return BadRequest(new { message = "User must confirm their age." });
            }

            if (userDto.TermsAgreement == false)
            {
                return BadRequest(new { message = "User must agree to the terms." });
            }

            var createdUser = await userService.AddUserAsync(UserDto.MapUserDtoToUser(userDto));
            if (createdUser == null)
            {
                return BadRequest(new { message = "Failed to create user." });
            }

            return Ok(createdUser);
        }

        [HttpPut("ResetUserPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailRequestDTO user)
        {
            if(string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Email is required.");
            }

            var generatedPassword = await userService.ResetPasswordAsync(user);
            if (generatedPassword == null)
            {
                return BadRequest(new { message = "Failed to update user." });
            }

            return Ok(new { tempPassword = generatedPassword });
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var updatedUser = await userService.UpdateUserAsync(UserDto.MapUserDtoToUser(userDto));
            if (updatedUser == null)
            {
                return BadRequest(new { message = "Failed to update user." });
            }

            return Ok(updatedUser);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userService.DeleteUserAsync(id);

            return NoContent();
        }
    }
}
