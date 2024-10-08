﻿using AgentsRest.Models;
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
        public DbSet<MissionModel> Missions { get; set; }
        public DbSet<EstimatedDurationsModel> HistoricalTimeLeft { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent) // Link each mission to Agent
                .WithMany() // Link the Agent to List of missions // H: a => a.AgentsMissions
                .HasForeignKey(m => m.AgentId) // Defining a Foreign key between the mission and the agent
                .OnDelete(DeleteBehavior.Restrict); // Restricting the deletion of shared information

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target) // Link each mission to Target
                .WithMany() // There is no link between Target and mission or anything else
                .HasForeignKey(m => m.TargetId) // Defining a Foreign key between the mission and the target
                .OnDelete(DeleteBehavior.Restrict); // Restricting the deletion of shared information


            modelBuilder.Entity<EstimatedDurationsModel>()
                .HasOne(h => h.Mission) // Link each historical time record to Mission
                .WithMany(m => m.HistoryTimeLeft) // Link The Mission to List of historical time record
                .HasForeignKey(m => m.MissionId) // Defining a Foreign key between the mission and the historical time record
                .OnDelete(DeleteBehavior.Restrict); // Restricting the deletion of shared information

            // Setting to save the statuses(Agent, Target, Mission) in text instead of int number
            modelBuilder.Entity<AgentModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();
            
            modelBuilder.Entity<MissionModel>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .IsRequired();

            base.OnModelCreating(modelBuilder); // To prevent unexpected behavior beyond the specific definitions above
        }
    }
}
