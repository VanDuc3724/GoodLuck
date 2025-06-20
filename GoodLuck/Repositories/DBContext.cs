using Microsoft.EntityFrameworkCore;
using GoodLuck.Models;

namespace GoodLuck.Repositories
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Anniversary> Anniversaries => Set<Anniversary>();
        public DbSet<Photo> Photos => Set<Photo>();
    }
}
