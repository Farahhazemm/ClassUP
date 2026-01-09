using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassUP.Infrastructure.Contexts.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            #region My Properties
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);


            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.Bio)
                .HasMaxLength(2000)
                .IsRequired(false);
            #endregion

            
            #region Relationships
      
            
            builder.HasMany(u => u.Courses)
                .WithOne(c => c.Instructor) 
                .HasForeignKey(c => c.InstructorId) 
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(u => u.Enrollments)
                .WithOne(e => e.User) 
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

             
            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User) 
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(u => u.Wishlists)
                .WithOne(w => w.User) 
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User) 
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            builder.HasMany(u => u.Payments)
                .WithOne(p => p.User) 
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

           
            builder.HasIndex(u => u.Email)
                .IsUnique(); 
            #endregion
        }
    }
}
