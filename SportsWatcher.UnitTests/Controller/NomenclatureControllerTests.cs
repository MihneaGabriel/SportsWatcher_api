using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsWatcher.WebApi.Controllers;
using SportsWatcher.WebApi.Entities.Nomenclature;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.UnitTests.Controller
{
    public class NomenclatureControllerTests
    {
        private readonly Mock<INomenclatureService> _mockNomenclatureService;
        private readonly NomenclatureController _controller;

        public NomenclatureControllerTests()
        {
            _mockNomenclatureService = new Mock<INomenclatureService>();
            _controller = new NomenclatureController(_mockNomenclatureService.Object);
        }

        [Fact]
        public async Task GetAllCountries_ReturnsOkResult_WithListOfCountries()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Id = 8, Key = "DZ", Value = "Algeria", isActive = true },
                new Country { Id = 2, Key = "CA", Value = "Canada", isActive = true }
            };

            _mockNomenclatureService
                .Setup(service => service.GetAllCountriesAsync())
                .ReturnsAsync(countries);

            // Act
            var result = await _controller.GetAllCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Country>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetAllCountries_ReturnsEmptyList_WhenNoCountriesExist()
        {
            // Arrange
            _mockNomenclatureService
                .Setup(service => service.GetAllCountriesAsync())
                .ReturnsAsync(new List<Country>());

            // Act
            var result = await _controller.GetAllCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Country>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }

}
