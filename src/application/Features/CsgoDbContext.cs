using csgo_stats.Models;
using domain.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace application.Features
{
    public class CsgoDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Kills> Kills { get; set; }
        public CsgoDbContext(DbContextOptions<CsgoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
            });

            builder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            builder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);
            });

            builder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.Id);
            });

            builder.Entity<Kills>(entity =>
            {
                entity.HasKey(k => new { k.PlayerId, k.MatchId });
            });
        }
    }
}