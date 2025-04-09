using SportsWatcher.WebApi.Data;
using SportsWatcher.WebApi.Interfaces;
using SportsWatcher.WebApi.Services;

namespace SportsWatcher.WebApi.Configuration
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICsvParserService, CsvParserService>();
            services.AddScoped<IOllamaService, OllamaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INomenclatureService, NomencaltureService>();

            services.AddHttpClient<IOllamaService, OllamaService>();

            return services;
        }
    }
}
