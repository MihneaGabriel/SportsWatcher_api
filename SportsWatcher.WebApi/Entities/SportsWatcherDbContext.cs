using Microsoft.EntityFrameworkCore;

namespace SportsWatcher.WebApi.Entities
{
    public class SportsWatcherDbContext(DbContextOptions<SportsWatcherDbContext> options) : DbContext(options)
    {
        public DbSet<AiResponse> AiResponse { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
