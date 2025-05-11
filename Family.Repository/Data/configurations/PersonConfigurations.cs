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
    public class PersonConfigurations : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            // Existing properties
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PhotoUrl).IsRequired().HasMaxLength(255);
            builder.Property(p => p.FatherName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.MotherName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.FacebookAccount).HasMaxLength(100);
            builder.Property(p => p.InstagramAccount).HasMaxLength(100);

            // New properties
            builder.Property(p => p.EmailAddress).IsRequired().HasMaxLength(100);
            builder.Property(p => p.AddressTitle).HasMaxLength(255);
            builder.Property(p => p.FCMToken).HasMaxLength(255);

 

           
        }
    }
}
