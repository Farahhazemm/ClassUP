using ClassUP.ApplicationCore.IRepository;
using ClassUP.Domain.Models;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    // Inject UserManager here
    public UnitOfWork(AppDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        Courses = new CourseRepository(_db);
        Lectures = new LectureRepository(_db);
        Categorises = new CategoryRepository(_db);
        Sections = new SectionRepository(_db);
        Reviews = new ReviewRepository(_db);
        Enrollments = new EnrollmentRepository(_db);
        Progresses = new ProgressRepository(_db);

        // Correct way to initialize Users repo
        Users = new UserRepository(_db, userManager);
    }

    public ICourseRepository Courses { get; }
    public ILectureRepository Lectures { get; }
    public ICategoryRepository Categorises { get; }
    public ISectionRepository Sections { get; }
    public IReviewRepository Reviews { get; }
    public IEnrollmentRepository Enrollments { get; }
    public IProgressRepository Progresses { get; }
    public IUserRepository Users { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}