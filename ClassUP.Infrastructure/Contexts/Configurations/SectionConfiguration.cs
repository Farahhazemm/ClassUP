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
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Sections");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                   .IsRequired()
                   .HasMaxLength(200);

           

            builder.Property(s => s.OrderIndex)
                   .IsRequired();

            builder.HasOne(s => s.Course)
                   .WithMany(c => c.Sections)
                   .HasForeignKey(s => s.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Lectures)
                   .WithOne(l => l.Section)
                   .HasForeignKey(l => l.SectionId)
                   .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
