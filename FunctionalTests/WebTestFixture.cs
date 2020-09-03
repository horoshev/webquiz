using Application.Entities;
using Data;
using FunctionalTests.Data.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web;
using Config = Web.IdentityServerConfiguration;

namespace FunctionalTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureServices(ConfigureServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RemoveAll<DbContextOptions<WebQuizDbContext>>();
            services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<WebQuizDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("app");
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopeServices = scope.ServiceProvider;
            var context = scopeServices.GetRequiredService<WebQuizDbContext>();
            var userManager = scopeServices.GetRequiredService<UserManager<User>>();
            QuestionContextSeed.SeedAsync(userManager, context).Wait();
        }
    }
}