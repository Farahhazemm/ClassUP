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
    public class CourseRequirementConfiguration : IEntityTypeConfiguration<CourseRequirement>
    {
        public void Configure(EntityTypeBuilder<CourseRequirement> builder)
        {
            builder.ToTable("CourseRequirements");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.RequirementText)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(cr => cr.OrderIndex)
                   .IsRequired();

            builder.HasOne(cr => cr.Course)
                   .WithMany(c => c.Requirements)
                   .HasForeignKey(cr => cr.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
