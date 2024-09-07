using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options) { }

        public DbSet<Post> Posts { get; set; }

        // public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        // public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add configurations here (if needed)
            base.OnModelCreating(modelBuilder);
        }
    }
}
