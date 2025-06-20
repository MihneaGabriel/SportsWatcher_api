﻿using SportsWatcher.WebApi.Entities.Nomenclature;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface INomenclatureService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
