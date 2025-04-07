using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsWatcher.WebApi.Controllers;
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

            // Act
            var result = await _controller.UploadCsv(_mockFormFile.Object);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UploadFile_EmptyFile_ReturnsBadRequest()
        {
            // Arrange
            _mockFormFile.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.UploadCsv(_mockFormFile.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
