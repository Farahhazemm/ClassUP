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
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            
            builder.ToTable("Enrollments");

         
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            
            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(e => e.CourseId)
                .IsRequired();

         
            builder.Property(e => e.EnrolledAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); 

            builder.Property(e => e.CompletedAt)
                .IsRequired(false); 

            builder.HasIndex(e => new { e.UserId, e.CourseId })
                .IsUnique();

            builder.Property(e => e.LastAccessedAt)
                .IsRequired(false);


            builder.Property(e => e.ProgressPercentage)
                .IsRequired()
                .HasDefaultValue(0);
                

            #region Relationships
           

            builder.HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.LectureProgresses)
                .WithOne(lp => lp.enrollment)
                .HasForeignKey(lp => lp.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            
           
        }
    }
}
