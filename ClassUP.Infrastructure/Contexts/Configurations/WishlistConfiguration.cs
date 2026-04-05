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
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("Wishlists");

            // PK (from BaseEntity)
            builder.HasKey(w => w.Id);

            // BaseEntity fields
            builder.Property(w => w.CreatedOn)
                   .IsRequired();

            builder.Property(w => w.UpdatedOn)
                   .IsRequired(false);

            // Relationships
            builder.HasOne(w => w.User)
                   .WithMany(u => u.Wishlists)
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Course)
                   .WithMany(c => c.CourseWishlists)
                   .HasForeignKey(w => w.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicates
            builder.HasIndex(w => new { w.UserId, w.CourseId })
                   .IsUnique();
        }
    }
}

