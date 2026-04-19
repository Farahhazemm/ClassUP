using ClassUP.ApplicationCore.Services.Categorise;
using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.ApplicationCore.Services.Enrollment;
using ClassUP.ApplicationCore.Services.Lectures;
using ClassUP.ApplicationCore.Services.LectursProgress;
using ClassUP.ApplicationCore.Services.Reviws;
using ClassUP.ApplicationCore.Services.Sections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassUP.ApplicationCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationConfig(this IServiceCollection services,
        IConfiguration configuration)
        {

            #region Services
            services.AddScoped<ICategoryServices, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IProgressService, ProgressService>();

            #endregion


            return services;
        }
    }
}
