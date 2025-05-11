using Family.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Family.Repository.Data.configurations
{
    public class BranchConfigurations : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(b => b.Name)
             .IsRequired()
             .HasMaxLength(100);

            builder.Property(b => b.Region)
             .IsRequired()
             .HasMaxLength(100);

            // Configure one-to-many relationship with Clan
            builder.HasOne(b => b.Clan)
                .WithMany(c => c.Branches)
                .HasForeignKey(b => b.ClanId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure one-to-many relationship with Person
            builder.HasMany(b => b.Persons)
                .WithOne(p => p.Branch)
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
