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
    public class VideoContentConfiguration : IEntityTypeConfiguration<VideoContent>
    {
        public void Configure(EntityTypeBuilder<VideoContent> builder)
        {
            builder.ToTable("VideoContents");

            // PK
            builder.HasKey(v => v.Id);

            // Properties
            builder.Property(v => v.VideoUrl)
                   .IsRequired()
                   .HasMaxLength(2048);

            builder.Property(v => v.PublicId)
                   .IsRequired()
                   .HasMaxLength(255);

            // BaseEntity
            builder.Property(v => v.CreatedOn)
                   .IsRequired();

            builder.Property(v => v.UpdatedOn)
                   .IsRequired(false);

            // Relationship (1:1)
            builder.HasOne(v => v.lecture)
                   .WithOne(l => l.VideoContent)
                   .HasForeignKey<VideoContent>(v => v.LectureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
