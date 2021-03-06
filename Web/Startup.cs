using System.Threading.Tasks;
using Application.Entities;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity;
using Unity.Lifetime;
using Web.Common;
using Config = Web.IdentityServerConfiguration;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebQuizDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;

                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebQuizDbContext>()
                .AddClaimsPrincipalFactory<WebQuizUserClaimsPrincipalFactory>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            // var credential = new X509Certificate2(File.ReadAllBytes("webquiz.pfx"), "password");
            services.AddIdentityServer()
                .AddApiAuthorization<User, WebQuizDbContext>();
            //    .AddTestUsers(Config.TestUsers);
            //    .AddSigningCredential(credential)
            //    .AddInMemoryClients(new []
            //    {
            //        new Client
            //        {
            //            ClientId = "Web",
            //            ClientName = "Web",
            //            AllowedGrantTypes = GrantTypes.Code,
            //   
            //            AllowOfflineAccess = true,
            //            AllowedScopes = { "openid", "profile", "WebAPI" }
            //        }
            //    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(UserPolicy), policy => policy.Requirements = UserPolicy.Requirements);
                options.AddPolicy(nameof(QuestionPolicy), policy => policy.Requirements = QuestionPolicy.Requirements);
            });

            services.AddMetrics();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            SeedData(services).Wait();
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>(TransientLifetimeManager.Instance);
            container.RegisterType<IQuestionService, QuestionService>(TransientLifetimeManager.Instance);
            // container.RegisterType<IAuthorizationHandler, QuestionOperationsAuthorizationHandler>(TransientLifetimeManager.Instance);
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
            if (env.IsDevelopment() || env.IsEnvironment("Testing"))
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            // app.UseAuthor();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            if (env.IsEnvironment("Testing")) return;

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer("start");
                }
            });
        }
    }
}