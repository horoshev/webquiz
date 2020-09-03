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
            ConfigureDatabase(services);

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
                .AddEntityFrameworkStores<WebQuizDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<User, WebQuizDbContext>();

            services.AddAuthorization(options =>
                options.AddPolicy(nameof(QuestionPolicy),
                    policy => policy.Requirements = QuestionPolicy.Requirements));

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddMetrics();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<WebQuizDbContext>(builder =>
                builder.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>(TransientLifetimeManager.Instance);
            container.RegisterType<IQuestionService, QuestionService>(TransientLifetimeManager.Instance);
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
            app.UseIdentityServer();
            app.UseAuthorization();

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