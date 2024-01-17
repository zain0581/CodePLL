using codepulse.API.Modells.Domain;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Data
{
    public class dbContext : DbContext

    {
        public dbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<BlogImage> BlogImages { get; set; }

    }
}
