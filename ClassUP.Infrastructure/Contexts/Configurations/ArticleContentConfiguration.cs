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
    public class ArticleContentConfiguration : IEntityTypeConfiguration<ArticleContent>
    {
        public void Configure(EntityTypeBuilder<ArticleContent> builder)
        {
            builder.ToTable("ArticleContents");

            builder.HasKey(ac => ac.Id);

            builder.Property(ac => ac.Content)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(ac => ac.ReadingTime)
                   .IsRequired();

            builder.HasOne(ac => ac.lecture)
                   .WithOne(l => l.ArticleContent)
                   .HasForeignKey<ArticleContent>(ac => ac.LectureId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}