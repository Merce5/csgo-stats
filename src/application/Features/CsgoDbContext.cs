using domain.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace application.Features
{
    public class CsgoDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public CsgoDbContext(DbContextOptions<CsgoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
            });
        }
    }
}