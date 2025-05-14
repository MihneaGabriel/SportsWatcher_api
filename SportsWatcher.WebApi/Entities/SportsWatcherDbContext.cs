using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SportsWatcher.WebApi.Entities.Nomenclature;

namespace SportsWatcher.WebApi.Entities
{
    public class SportsWatcherDbContext(DbContextOptions<SportsWatcherDbContext> options) : DbContext(options)
    {
        public DbSet<AiResponse> AiResponse { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
