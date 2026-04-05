using ClassUP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ArticleContentConfiguration : IEntityTypeConfiguration<ArticleContent>
{
    public void Configure(EntityTypeBuilder<ArticleContent> builder)
    {
        builder.ToTable("ArticleContents");

        builder.HasKey(ac => ac.Id);

        builder.Property(ac => ac.Content)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(ac => ac.CreatedOn)
               .IsRequired();

        builder.Property(ac => ac.UpdatedOn)
               .IsRequired(false);

        builder.HasOne(ac => ac.lecture)
               .WithOne(l => l.ArticleContent)
               .HasForeignKey<ArticleContent>(ac => ac.LectureId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}