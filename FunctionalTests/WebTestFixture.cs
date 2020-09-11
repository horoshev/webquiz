using System.Net.Http;
using System.Net.Http.Headers;
using Application.Entities;
using AutoMapper;
using Data;
using FunctionalTests.Data.Seed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Web.Common;
using Config = Web.IdentityServerConfiguration;

namespace FunctionalTests
{
    public class WebTestFixture : WebApplicationFactory<StartupTesting>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing")
                .ConfigureTestServices(ConfigureServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();

            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<WebQuizDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("test");
                builder.UseInternalServiceProvider(provider);
                // builder.UseInMemoryDatabase($"test-{Guid.NewGuid()}");
            });

            services
                .AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebQuizDbContext>()
                .AddClaimsPrincipalFactory<WebQuizUserClaimsPrincipalFactory>();

            services.AddAuthentication("Testing")
                .AddScheme<AuthenticationSchemeOptions, TestingAuthHandler>("Testing", options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(UserPolicy), policy => policy.Requirements = UserPolicy.Requirements);
                options.AddPolicy(nameof(QuestionPolicy), policy => policy.Requirements = QuestionPolicy.Requirements);
            });

            services.AddAutoMapper(typeof(Startup));

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopeServices = scope.ServiceProvider;
            var context = scopeServices.GetRequiredService<WebQuizDbContext>();
            context.Database.EnsureCreated();
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