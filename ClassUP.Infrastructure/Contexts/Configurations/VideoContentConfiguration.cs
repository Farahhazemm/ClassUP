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


            builder.HasOne(v => v.lecture)
                   .WithOne(l => l.VideoContent)
                   .HasForeignKey<VideoContent>(v => v.LectureId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
