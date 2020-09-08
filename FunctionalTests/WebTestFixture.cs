using System.Net.Http;
using System.Net.Http.Headers;
using Application.Entities;
using Data;
using FunctionalTests.Data.Seed;
using Microsoft.AspNetCore.Authentication;
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
            // Re-configure database
            services.RemoveAll<DbContextOptions<WebQuizDbContext>>();
            services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<WebQuizDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("test");
                // builder.UseInMemoryDatabase($"test-{Guid.NewGuid()}");
                // builder.UseInternalServiceProvider(provider);
            });

            // Re-configure Identity
            services.AddAuthentication("Testing")
                .AddScheme<AuthenticationSchemeOptions, TestingAuthHandler>("Testing", options =>
                {

                });

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopeServices = scope.ServiceProvider;
            var context = scopeServices.GetRequiredService<WebQuizDbContext>();
            var userManager = scopeServices.GetRequiredService<UserManager<User>>();
            QuestionContextSeed.SeedAsync(userManager, context).Wait();
        }

        public static HttpClient AuthenticateUser(HttpClient user, string userId = "123")
        {
            user.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Testing");
            user.DefaultRequestHeaders.Add(nameof(userId), userId);

            return user;
        }
    }
}