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
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories");

            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(sc => sc.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.HasOne(sc => sc.Category)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sc => sc.CourseSubCategories)
                   .WithOne(cs => cs.SubCategory)
                   .HasForeignKey(cs => cs.SubCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

