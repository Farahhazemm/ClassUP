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
    public class CourseObjectiveConfiguration : IEntityTypeConfiguration<CourseObjective>
    {
        public void Configure(EntityTypeBuilder<CourseObjective> builder)
        {
            builder.ToTable("CourseObjectives");

            builder.HasKey(co => co.Id);

            builder.Property(co => co.ObjectiveText)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(co => co.OrderIndex)
                   .IsRequired();

            builder.HasOne(co => co.Course)
                   .WithMany(c => c.Objectives)
                   .HasForeignKey(co => co.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(co => new { co.CourseId, co.OrderIndex });
        }
    }
}
