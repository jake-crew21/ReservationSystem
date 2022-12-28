using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Models;
using ReservationSystem.Converters;

namespace ReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<SittingSchedule> SittingSchedule { get; set; }
        public DbSet<Sitting> Sitting { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Set HasKey for composite keys
            builder.Entity<SittingSchedule>().HasKey(k => new { k.Id });
            builder.Entity<Sitting>().HasKey(k => new { k.TableId, k.BookingId });
            //use OnModleCreating method to pre-pop your Db for marking purpose;
            builder.Entity<Table>(e => e.Property(e => e.Id).ValueGeneratedOnAdd());
            builder.Entity<Reservation>(e => e.Property(e => e.BookingId).ValueGeneratedOnAdd());
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
        //Configures data type of DateOnly -> Date and TimeOnly -> Time using converters
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>().HaveColumnType("date");
            builder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>().HaveColumnType("time");
        }
        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)//this line that identifies that these columns must be configured to the AspNetUsers table
            {
                builder.Property(u => u.FirstName).HasMaxLength(255);
                builder.Property(u => u.LastName).HasMaxLength(255);
            }
        }
    }
}