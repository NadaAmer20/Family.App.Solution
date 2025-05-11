using Family.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Repository.Data.configurations
{
    public class ClanConfigurations : IEntityTypeConfiguration<Clan>
    {
        public void Configure(EntityTypeBuilder<Clan> builder)
        {
            builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(c => c.Region)
                .IsRequired()
                .HasMaxLength(100);

            // Configure one-to-many relationship with Branch
            builder.HasMany(c => c.Branches)
                .WithOne(b => b.Clan)
                .HasForeignKey(b => b.ClanId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
