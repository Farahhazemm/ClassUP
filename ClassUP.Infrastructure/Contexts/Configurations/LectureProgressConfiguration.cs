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
    public class LectureProgressConfiguration : IEntityTypeConfiguration<LectureProgress>
    {
        public void Configure(EntityTypeBuilder<LectureProgress> builder)
        {
            builder.ToTable("LectureProgresses");

            builder.HasKey(lp => lp.Id);
            builder.Property(lp => lp.Id).ValueGeneratedOnAdd();

            builder.Property(lp => lp.EnrollmentId).IsRequired();
            builder.Property(lp => lp.LectureId).IsRequired();
            builder.Property(lp => lp.IsCompleted).IsRequired().HasDefaultValue(false);
          
            builder.Property(lp => lp.CompletedAt).IsRequired(false);

            builder.HasOne(lp => lp.enrollment)
                .WithMany(e => e.LectureProgresses)
                .HasForeignKey(lp => lp.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lp => lp.lecture)
                .WithMany(l => l.LectureProgresses)
                .HasForeignKey(lp => lp.LectureId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}
