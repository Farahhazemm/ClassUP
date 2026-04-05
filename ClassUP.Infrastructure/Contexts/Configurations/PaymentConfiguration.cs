using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            // PK
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.TransactionId)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.UserId)
                   .IsRequired();

            // BaseEntity
            builder.Property(p => p.CreatedOn)
                   .IsRequired();

            builder.Property(p => p.UpdatedOn)
                   .IsRequired(false);

            // Relationships

            builder.HasOne(p => p.User)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Order)
                   .WithOne(o => o.Payment)
                   .HasForeignKey<Payment>(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

