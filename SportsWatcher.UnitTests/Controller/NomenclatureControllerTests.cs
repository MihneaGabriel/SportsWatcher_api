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

        #region Countires TESTS
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
        #endregion


        #region Categories TESTS
        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Key = "football", Value = "Football", isActive = true },
                new Category { Id = 2, Key = "basketball", Value = "Basketball", isActive = true }
            };

            _mockNomenclatureService
                .Setup(service => service.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetAllCategories_ReturnsEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            _mockNomenclatureService
                .Setup(service => service.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Category>());

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Empty(returnValue);
        }
        #endregion

    }

}
