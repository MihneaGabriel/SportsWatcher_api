using SportsWatcher.WebApi.Entities.Nomenclature;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Services
{
    public class NomencaltureService(IGenericRepository<Country> taraRepository, IUnitOfWork uow ) : INomenclatureService
    {
        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var taraEntities = await taraRepository.GetAllAsync();
            return taraEntities;
        }
    }
}
