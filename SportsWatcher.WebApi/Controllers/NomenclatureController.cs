using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NomenclatureController(INomenclatureService nomenclatureService) : ControllerBase
    {
        #region Countries
        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await nomenclatureService.GetAllCountriesAsync();
            if (countries == null)
            {
                return NotFound(new { message = "No countries found." });
            }
            return Ok(countries);
        }
        #endregion
    }
}