//using Family.Core.Entities;
//using Family.Core.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace Family.Repository.Data
//{
//    public class AppIdentityDbContext : IdentityDbContext<AppUser>
//    {
//        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
//            : base(options)
//        {
//        }
 
//        public DbSet<Notifications> Notifications { get; set; }
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            builder.Entity<AppUser>()
//    .HasOne(u => u.Branch)
//    .WithMany(b => b.Persons)
//    .HasForeignKey(u => u.BranchId)
//    .OnDelete(DeleteBehavior.SetNull);
 

//            builder.Entity<Branch>()
//                .HasOne(b => b.Clan)
//                .WithMany(c => c.Branches)
//                .HasForeignKey(b => b.ClanId)
//                .OnDelete(DeleteBehavior.NoAction); // هذا هو المطلوب لتفادي الخطأ

//            builder.Entity<Notifications>()
//                .HasOne(n => n.Person)
//                .WithMany(u => u.Notifications)
//                .HasForeignKey(n => n.PersonId)
//                .OnDelete(DeleteBehavior.NoAction);
//        }

//    }
//}
