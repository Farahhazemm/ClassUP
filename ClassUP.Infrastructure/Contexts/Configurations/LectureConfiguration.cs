using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lectures");

        // PK
        builder.HasKey(l => l.Id);

        // Properties
        builder.Property(l => l.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(l => l.SectionId)
            .IsRequired();

        builder.Property(l => l.IsFree)
            .IsRequired()
            .HasDefaultValue(false);

        // BaseEntity (Audit fields)
        builder.Property(l => l.CreatedOn)
            .IsRequired();

        builder.Property(l => l.UpdatedOn)
            .IsRequired(false);

        // Relationships

        builder.HasOne(l => l.Section)
            .WithMany(s => s.Lectures)
            .HasForeignKey(l => l.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-One: VideoContent (optional)
        builder.HasOne(l => l.VideoContent)
            .WithOne(vc => vc.lecture)
            .HasForeignKey<VideoContent>(vc => vc.LectureId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // One-to-One: ArticleContent (optional)
        builder.HasOne(l => l.ArticleContent)
            .WithOne(ac => ac.lecture)
            .HasForeignKey<ArticleContent>(ac => ac.LectureId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // One-to-Many
        builder.HasMany(l => l.LectureProgresses)
            .WithOne(lp => lp.lecture)
            .HasForeignKey(lp => lp.LectureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}