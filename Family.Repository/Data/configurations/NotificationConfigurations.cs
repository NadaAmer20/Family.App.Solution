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
    public class NotificationConfigurations : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(n => n.Body)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.NotificationType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(n => n.Data)
                .HasMaxLength(2000);

            builder.Property(n => n.ImageUrl)
                .HasMaxLength(255);

            builder.Property(n => n.RedirectUrl)
                .HasMaxLength(255);

            builder.Property(n => n.FCMResponse)
                .HasMaxLength(255);

            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.Property(n => n.IsRead)
                .IsRequired();

            builder.Property(n => n.IsSent)
                .IsRequired();

            // Configure relationship with Person
            builder.HasOne(n => n.Person)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.HasIndex(n => n.PersonId);
            builder.HasIndex(n => n.CreatedAt);
            builder.HasIndex(n => n.IsRead);
        }
    }
}
