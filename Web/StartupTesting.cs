using System.Threading.Tasks;
using Application.Entities;
using Application.Interfaces;
using Application.Services;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unity;
using Unity.Lifetime;
using Web.Common;
using Config = Web.IdentityServerConfiguration;

namespace Web
{
    public class StartupTesting
    {
        public StartupTesting(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>(TransientLifetimeManager.Instance);
            container.RegisterType<IQuestionService, QuestionService>(TransientLifetimeManager.Instance);
        }

        protected virtual async Task SeedData(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var scopeServices = scope.ServiceProvider;
            var context = scopeServices.GetRequiredService<WebQuizDbContext>();
            await context.Database.EnsureCreatedAsync();

            var userManager = scopeServices.GetRequiredService<UserManager<User>>();
            var roleManager = scopeServices.GetRequiredService<RoleManager<IdentityRole>>();

            await ConfigureRoles(roleManager);
            await ConfigureUsers(userManager);
        }

        private static async Task ConfigureRoles(RoleManager<IdentityRole> roleManager)
        {
            await ContextSeeder.SeedRolesAsync(roleManager);
        }

        private static async Task ConfigureUsers(UserManager<User> userManager)
        {
            await ContextSeeder.SeedAdminUser(userManager);
            await ContextSeeder.SeedCommonUser(userManager);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}