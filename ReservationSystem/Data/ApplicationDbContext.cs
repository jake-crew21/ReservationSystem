using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.Models;
using ReservationSystem.Converters;

namespace ReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<SittingSchedule> SittingSchedule { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Set HasKey for composite keys
            builder.Entity<Reservation>().HasKey(k => new { k.Contact, k.ResDate, k.StartTime });
            builder.Entity<SittingSchedule>().HasKey(k => new { k.SessionType });
            //use OnModleCreating method to pre-pop your Db for marking purpose;
            builder.Entity<AccountType>(e => e.Property(e => e.Id).ValueGeneratedOnAdd());
            builder.Entity<Table>(e => e.Property(e => e.Id).ValueGeneratedOnAdd());
            //This is also called seed data meaning when migration happens not only tables are created but rows are added too
            builder.Entity<AccountType>().HasData(
                new AccountType
                {
                    Id = 1,
                    Name = "Standard"
                },
                new AccountType
                {
                    Id = 2,
                    Name = "Admin"
                }
            );
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