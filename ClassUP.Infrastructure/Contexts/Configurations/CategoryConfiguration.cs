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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");


            #region My Properties
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.IconUrl)
                .IsRequired()
                .HasMaxLength(500);

            #endregion


            #region Relationships

            if (typeof(Category).GetProperty("SubCategories") != null)
            {
                builder.HasMany(c => c.SubCategories)
                    .WithOne(s => s.Category) 
                    .HasForeignKey(s => s.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade); 
            }

            
            if (typeof(Category).GetProperty("Courses") != null)
            {
                builder.HasMany(c => c.Courses)
                    .WithOne(co => co.Category) 
                    .HasForeignKey(co => co.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict); 
            }

            #endregion
            
            
        }
    }
}
