﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsWatcher.WebApi.Controllers;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.UnitTests.Controller
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        #region CRUD TESTS
        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserDto>
                {
                    new UserDto { Username = "John Doe", Password = "12345678", Email = "john.doe@example.com", Country = "Romania" },
                    new UserDto { Username = "Jane Doe", Password = "87654321", Email = "jane.smith@example.com", Country = "Romania" }
                };

            _mockUserService
                .Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnNotFoundObjectResult_WhenNoUsersExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_ShouldReturnOkResult_WithUser()
        {
            // Arrange
            var user = new UserDto { Username = "Jane Doe", Password = "12345678", Email = "john.doe@example.com", Country = "Romania" };
            var mocklogin = new LoginDto { Email = "john.doe@example.com", Password = "123456789" };
            _mockUserService.Setup(service => service.GetUserAsync(mocklogin)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(mocklogin);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("Jane Doe", returnedUser.Username);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFoundObjectResult_WhenUserDoesNotExist()
        {
            // Arrange
            var mocklogin = new LoginDto { Email = "john.doe@example.com", Password = "123456789" };
            _mockUserService.Setup(service => service.GetUserAsync(mocklogin)).ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.GetUser(mocklogin);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnOkObjectResult_WithCreatedUser()
        {
            // Arrange
            var userDto = new UserDto { Username = "John Doe", Password = "12345678", Email = "john.doe@example.com", Country = "Romania", AgeConfirmation = true, TermsAgreement = true };
            var user = new User { UserFirstName = "John", UserLastName = "Doe", Username = "johnDoe",  PasswordHash = "87654331", UserEmail = "john.doe@example.com", Country = "Romania" };
            _mockUserService.Setup(service => service.AddUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(createdResult.Value);
            Assert.Equal("John", returnedUser.UserFirstName);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserEmail", "Required");

            // Act
            var result = await _controller.CreateUser(new UserDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOkResult_WithUpdatedUser()
        {
            // Arrange
            var updatedUser = new User
            {
                Id = 1,
                UserFirstName = "John",
                UserLastName = "Doe",
                Username = "johnDoe",
                PasswordHash = "12345678",
                UserEmail = "john.doe@example.com",
                Country = "Romania"
            };

            _mockUserService
                .Setup(service => service.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(UserDto.MapUserToUserDto(updatedUser));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(updatedUser.UserFirstName, returnedUser.UserFirstName);
            Assert.Equal(updatedUser.UserLastName, returnedUser.UserLastName);
            Assert.Equal(updatedUser.UserEmail, returnedUser.UserEmail);
            Assert.Equal(updatedUser.Country, returnedUser.Country);

            _mockUserService
                .Verify(service => service.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnBadRequestObjectResult_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService.Setup(service => service.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateUser(new UserDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.DeleteUserAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContentResult_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.DeleteUserAsync(1));

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert

            Assert.IsType<NoContentResult>(result);

        }
        #endregion

        #region Password Tests
        [Fact]
        public async Task ResetPassword_ShouldReturnNotFoundResult_WhenEmailDoesNotExist()
        {
            // Arrange
            var emailRequest = new EmailRequestDTO { Email = "nonexistent@example.com" };
            _mockUserService
                .Setup(service => service.ResetPasswordAsync(emailRequest))
                .ReturnsAsync((string)null);

            // Act
            var result = await _controller.ResetPassword(emailRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ResetPassword_ShouldReturnOkResult_WithTempPassword()
        {
            // Arrange
            var emailRequest = new EmailRequestDTO { Email = "john.doe@example.com" };
            var generatedPassword = "Temp12345!";

            _mockUserService
                .Setup(service => service.ResetPasswordAsync(emailRequest))
                .ReturnsAsync(generatedPassword);

            // Act
            var result = await _controller.ResetPassword(emailRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var resultDict = okResult.Value
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

            Assert.True(resultDict.ContainsKey("tempPassword"));
            Assert.Equal(generatedPassword, resultDict["tempPassword"]);
        }
        #endregion
    }
}
