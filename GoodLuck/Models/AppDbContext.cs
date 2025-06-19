using Microsoft.EntityFrameworkCore;

namespace GoodLuck.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Letter> Letters => Set<Letter>();
        public DbSet<Anniversary> Anniversaries => Set<Anniversary>();
    }
}
