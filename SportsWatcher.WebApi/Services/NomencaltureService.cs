using SportsWatcher.WebApi.Entities.Nomenclature;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Services
{
    public class NomencaltureService(IGenericRepository<Country> countryRepository) : INomenclatureService
    {
        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countryEntities = await countryRepository.GetAllAsync();
            return countryEntities;
        }
    }
}
