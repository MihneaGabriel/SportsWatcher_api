using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsWatcher.WebApi.Controllers;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.UnitTests.Controllers
{
    public class FileUploadControllerTests
    {
        private readonly Mock<IFormFile> _mockFormFile;
        private readonly FileUploadController _controller;

        public FileUploadControllerTests()
        {
            _mockFormFile = new Mock<IFormFile>();
            var mockCsvParserService = new Mock<ICsvParserService>();
            var mockOllamaService = new Mock<IOllamaService>();
            _controller = new FileUploadController(mockCsvParserService.Object, mockOllamaService.Object);
        }

        [Fact]
        public async Task UploadFile_ValidFile_ReturnsOkResult()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            _mockFormFile.Setup(f => f.OpenReadStream()).Returns(ms);
            _mockFormFile.Setup(f => f.FileName).Returns(fileName);
            _mockFormFile.Setup(f => f.Length).Returns(ms.Length);

            var aiResponse = new AiResponseDto
            {
                File = _mockFormFile.Object,
                UserId = 1,
                CategoryId = 1,
            };

            // Act
            var result = await _controller.UploadCsv(aiResponse);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UploadFile_EmptyFile_ReturnsBadRequest()
        {
            // Arrange
            _mockFormFile.Setup(f => f.Length).Returns(0);

            var aiResponse = new AiResponseDto
            {
                File = _mockFormFile.Object,
                UserId = 1,
                CategoryId = 1,
                
            };

            // Act
            var result = await _controller.UploadCsv(aiResponse);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]

        public async Task GetAiResponse_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var mockCsvParserService = new Mock<ICsvParserService>();
            var mockOllamaService = new Mock<IOllamaService>();
            var controller = new FileUploadController(mockCsvParserService.Object, mockOllamaService.Object);
            int userId = 1;
            int categoryId = 2;
            var aiResponses = new List<AiResponse>();
            mockOllamaService.Setup(s => s.GetAiResponsesAsync(userId, categoryId))
                .ReturnsAsync(aiResponses);

            // Act
            var result = await controller.GetAiResponse(userId, categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(aiResponses, okResult.Value);
        }

        [Fact]
        public async Task GetAiResponse_ServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var mockCsvParserService = new Mock<ICsvParserService>();
            var mockOllamaService = new Mock<IOllamaService>();
            var controller = new FileUploadController(mockCsvParserService.Object, mockOllamaService.Object);
            int userId = 1;
            int categoryId = 2;
            mockOllamaService.Setup(s => s.GetAiResponsesAsync(userId, categoryId))
                .ReturnsAsync((List<AiResponse>?)null);

            // Act
            var result = await controller.GetAiResponse(userId, categoryId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


    }
}
