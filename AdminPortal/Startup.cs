

using AdminPortal.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdminPortal
{
    public class Startup
    {
        public AppSettings appsettings = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable session storage
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();  // Required for HttpContext in services
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<TokenAuthorizationFilter>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

        }

        public void Configure(IApplicationBuilder app)
        {
            // Other configurations
            app.UseDeveloperExceptionPage();
            
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                // Define the default route pattern
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");

                // If using Razor Pages, you can also map Razor Pages
                endpoints.MapRazorPages();
            });

        }
    }
}
