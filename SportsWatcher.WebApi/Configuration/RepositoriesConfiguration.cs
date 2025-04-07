using SportsWatcher.WebApi.Interfaces;
using SportsWatcher.WebApi.Services;

namespace SportsWatcher.WebApi.Configuration
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICsvParserService, CsvParserService>();
            services.AddScoped<IOllamaService, OllamaService>();

            services.AddHttpClient<IOllamaService, OllamaService>();

            return services;
        }
    }
}
