using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Application.Entities;
using Data;
using FunctionalTests.Data.Seed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
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
                builder.UseInMemoryDatabase("app");
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

        public static HttpClient AuthenticateUser(HttpClient user)
        {
            // ToDo: Set user identifier claims
            user.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Testing");

            return user;
        }

/*
        public static void AuthenticateUser(HttpClient user)
        {
            user.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue();
        }

        private string GenerateToken(IDataProtector protector)
        {
            // Generate an OAuth bearer token for ASP.NET/Owin Web Api service that uses the default OAuthBearer token middleware.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "WebApiUser"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, "PowerUser"),
            };
            var identity = new ClaimsIdentity(claims, "Test");

            // Use the same token generation logic as the OAuthBearer Owin middleware.
            var tdf = new TicketDataFormat(protector);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddHours(1) });
            var accessToken = tdf.Protect(ticket);

            return accessToken;
        }
        */
    }
}