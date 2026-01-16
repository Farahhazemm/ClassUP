using ClassUP.ApplicationCore.Services.Courses;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ClassUP.ApplicationCore
{
    public static class DependencyInjection   
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();

            return services;
        }
    }
}
