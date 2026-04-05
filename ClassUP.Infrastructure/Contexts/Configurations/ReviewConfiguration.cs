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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            // PK
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.Rating)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .IsRequired()
                   .HasMaxLength(2000);

            // BaseEntity
            builder.Property(r => r.CreatedOn)
                   .IsRequired();

            builder.Property(r => r.UpdatedOn)
                   .IsRequired(false);

            // Relationships

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Course)
                   .WithMany(c => c.Reviews)
                   .HasForeignKey(r => r.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
