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
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            
            builder.ToTable("Lectures");

           
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id)
                .ValueGeneratedOnAdd();

           
            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(l => l.Type)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>();

         
            builder.Property(l => l.Duration)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(l => l.OrderIndex)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(l => l.SectionId)
                .IsRequired();

           
            builder.Property(l => l.IsFree)
                .IsRequired()
                .HasDefaultValue(false);

            
            builder.HasOne(l => l.Section)
                .WithMany(s => s.Lectures)
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasOne(l => l.VideoContent)
                .WithOne(vc => vc.lecture)
                .HasForeignKey<VideoContent>(vc => vc.LectureId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

          
            builder.HasOne(l => l.ArticleContent)
                .WithOne(ac => ac.lecture)
                .HasForeignKey<ArticleContent>(ac => ac.LectureId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            
            builder.HasMany(l => l.LectureProgresses)
                .WithOne(lp => lp.lecture)
                .HasForeignKey(lp => lp.LectureId)
                .OnDelete(DeleteBehavior.Cascade);

            

           
        }
    }
}