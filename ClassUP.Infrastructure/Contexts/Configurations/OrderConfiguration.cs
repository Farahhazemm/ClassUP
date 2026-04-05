using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassUP.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            // PK
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.Total)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .IsRequired();

            // BaseEntity mapping
            builder.Property(o => o.CreatedOn)
                   .IsRequired();

            builder.Property(o => o.UpdatedOn)
                   .IsRequired(false);

            // FK
            builder.Property(o => o.UserId)
                   .IsRequired();

            // Relationships

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Payment)
                   .WithOne(p => p.Order)
                   .HasForeignKey<Payment>(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}

