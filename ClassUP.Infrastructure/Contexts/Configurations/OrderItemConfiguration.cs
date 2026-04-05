using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        // PK
        builder.HasKey(oi => oi.Id);

        // Properties
        builder.Property(oi => oi.CourseTitle)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(oi => oi.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.CourseId)
               .IsRequired();

        // BaseEntity
        builder.Property(oi => oi.CreatedOn)
               .IsRequired();

        builder.Property(oi => oi.UpdatedOn)
               .IsRequired(false);

        // Relationships
        builder.HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        //  Course relationship
        builder.HasOne<Course>()
               .WithMany()
               .HasForeignKey(oi => oi.CourseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}