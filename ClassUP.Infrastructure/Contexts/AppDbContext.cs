using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts.Configurations;
using ClassUP.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ClassUP.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<VideoContent> VideoContents { get; set; }
        public DbSet<ArticleContent> ArticleContents { get; set; }
        public DbSet<CourseRequirement> CourseRequirements { get; set; }
        public DbSet<CourseObjective> CourseObjectives { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<LectureProgress> LessonProgresses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTagConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new LectureConfiguration());
            modelBuilder.ApplyConfiguration(new VideoContentConfiguration());
            
            modelBuilder.ApplyConfiguration(new ArticleContentConfiguration());
            modelBuilder.ApplyConfiguration(new CourseRequirementConfiguration());
            modelBuilder.ApplyConfiguration(new CourseObjectiveConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
           modelBuilder.ApplyConfiguration(new LectureProgressConfiguration());
            
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new WishlistConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        }
    }
}
