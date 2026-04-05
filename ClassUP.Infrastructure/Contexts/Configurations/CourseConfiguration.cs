using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Contexts.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(c => c.Level)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(c => c.ThumbnailUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.PreviewVideoUrl)
                .HasMaxLength(500);

            builder.Property(c => c.IsPublished)
                .HasDefaultValue(false);

            builder.Property(c => c.PublishedAt)
                .IsRequired(false);

            // FK
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.CategoryId).IsRequired();

            // Audit (BaseEntity)
            builder.Property(c => c.CreatedOn).IsRequired();
            builder.Property(c => c.UpdatedOn).IsRequired(false);

            // Relations
            builder.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.user)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Sections)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Requirements)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Objectives)
                .WithOne(o => o.Course)
                .HasForeignKey(o => o.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CourseTags)
                .WithOne(t => t.Course)
                .HasForeignKey(t => t.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Reviews)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
