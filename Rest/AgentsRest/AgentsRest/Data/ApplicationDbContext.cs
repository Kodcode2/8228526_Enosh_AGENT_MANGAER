using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AgentsApi.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IConfiguration configuration
        ) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<MissionModel> Missions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent)
                .WithMany()
                .HasForeignKey(m => m.AgentId)
                .OnDelete(DeleteBehavior.Restrict); // הגבלת מחיקה

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder); // למניעת התנהגות לא צפויה מעבר להגדרת הספציפיות לעיל
        }
    }
}
