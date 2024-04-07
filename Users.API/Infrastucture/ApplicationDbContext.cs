using Microsoft.EntityFrameworkCore;
using Users.API.Domain;

namespace Users.API.Infrastucture
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User => Set<User>();
    }
}
