using ClassUP.ApplicationCore.IRepository;
using ClassUP.ApplicationCore.Services.Auth;
using ClassUP.ApplicationCore.Services.IAccount;
using ClassUP.ApplicationCore.Services.IIdentity;
using ClassUP.ApplicationCore.Services.IImage;
using ClassUP.Infrastructure.Contexts;
using ClassUP.Infrastructure.Identity.Services;
using ClassUP.Infrastructure.Identity_Account.Services;
using ClassUP.Infrastructure.Repository;
using ClassUP.Infrastructure.Services.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassUP.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
           
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MyConc"),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
            );

            #region UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            #endregion

            #region Repositories
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IProgressRepository, ProgressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region IdentityServices
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();


            #endregion

            services.AddScoped<IImageValidator, ImageValidator>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            return services;
        }
    }
}
