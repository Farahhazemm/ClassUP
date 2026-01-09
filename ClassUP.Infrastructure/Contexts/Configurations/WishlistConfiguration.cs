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

            builder.HasKey(w => w.Id);

            builder.Property(w => w.AddedAt)
                   .IsRequired();

            builder.HasOne(w => w.User)
                   .WithMany(u => u.Wishlists)
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Course)
                   .WithMany(c => c.CourseWishlists)
                   .HasForeignKey(w => w.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(w => new { w.UserId, w.CourseId })
                   .IsUnique();
        }
    }
}

