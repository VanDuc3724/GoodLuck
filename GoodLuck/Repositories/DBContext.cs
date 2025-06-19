using Microsoft.EntityFrameworkCore;
using GoodLuck.Models;

namespace GoodLuck.Repositories
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Letter> Letters => Set<Letter>();
        public DbSet<Anniversary> Anniversaries => Set<Anniversary>();
    }
}
