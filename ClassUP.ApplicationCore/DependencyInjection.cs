using ClassUP.ApplicationCore.Services.Courses;
using ClassUP.ApplicationCore.Services.Lectures;
using ClassUP.ApplicationCore.Services.Thumbnail;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ClassUP.ApplicationCore
{
    public static class DependencyInjection   
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IThumbnailService, ThumbnailService>();
            services.AddScoped<ILectureService, LectureService>();


            return services;
        }
    }
}
