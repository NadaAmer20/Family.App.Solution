using Family.Core.Entities;
using Family.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Family.Repository.Data
{
    public class FamilyContext : IdentityDbContext<AppUser>
    {
        public FamilyContext(DbContextOptions<FamilyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // تكوين العلاقات
            modelBuilder.Entity<Clan>()
                .HasMany(c => c.Branches)
                .WithOne(b => b.Clan)
                .HasForeignKey(b => b.ClanId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Branch)
                .WithMany(b => b.Persons)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.Person)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.PersonId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        // DbSets
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public DbSet<SliderItem> SliderItems { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
    }
}