using ClassUP.API.Middlewares;
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Serilog;

namespace ClassUP.API
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApiPipeline(this WebApplication app)
        {
            app.UseCors("AllowAll");
            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRateLimiter();

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:username"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:password")
                    }
                ],
                DashboardTitle = "ClassUP Background Jobs",
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
