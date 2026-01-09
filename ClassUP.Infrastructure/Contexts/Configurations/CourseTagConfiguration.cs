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
    public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
    {
        public void Configure(EntityTypeBuilder<CourseTag> builder)
        {
            
            builder.ToTable("CourseTags"); 

           
            builder.HasKey(ct => new { ct.CourseId, ct.TagId });
           

            builder.HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTags) 
                .HasForeignKey(ct => ct.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
          


            builder.HasOne(ct => ct.Tag)
                .WithMany(t => t.CourseTags) 
                .HasForeignKey(ct => ct.TagId)
                .OnDelete(DeleteBehavior.Cascade);
            

            

            
        }
    }
}
