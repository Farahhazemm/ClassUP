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
    public class CourseSubCategoryConfiguration : IEntityTypeConfiguration<CourseSubCategory>
    {
        public void Configure(EntityTypeBuilder<CourseSubCategory> builder)
        {
            builder.ToTable("CourseSubCategories");

            builder.HasKey(cs => new { cs.CourseId, cs.SubCategoryId });

            builder.HasOne(cs => cs.Course)
                   .WithMany(c => c.CourseSubCategories)
                   .HasForeignKey(cs => cs.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.SubCategory)
                   .WithMany(sc => sc.CourseSubCategories)
                   .HasForeignKey(cs => cs.SubCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
