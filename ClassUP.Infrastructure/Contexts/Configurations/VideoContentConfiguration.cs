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

            builder.HasKey(v => v.Id);

            builder.Property(v => v.FileName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(v => v.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(v => v.VideoUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(v => v.Duration)
                   .IsRequired();

            builder.Property(v => v.FileSize)
                   .IsRequired();

            builder.Property(v => v.Quality)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(v => v.ThumbnailUrl)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(v => v.UploadedBy)
                   .IsRequired();

            builder.Property(v => v.UploadedAt)
                   .IsRequired();

            builder.HasOne(v => v.lecture)
                   .WithOne(l => l.VideoContent)
                   .HasForeignKey<VideoContent>(v => v.LectureId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
