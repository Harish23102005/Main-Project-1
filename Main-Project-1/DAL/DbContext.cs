using Microsoft.EntityFrameworkCore;

namespace MainProject1
{
    public class SustainabilityDbContext : DbContext
    {
        public SustainabilityDbContext(DbContextOptions<SustainabilityDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<ApplianceType> ApplianceTypes { get; set; }
        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<EnergyUsage> EnergyUsages { get; set; }
        public DbSet<WaterUsage> WaterUsages { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SustainabilityScore> SustainabilityScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Alert)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.AlertId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}