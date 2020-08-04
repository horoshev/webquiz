using System;
using Application.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data
{
    public class WebQuizDbContext : ApiAuthorizationDbContext<User>
    {
        public DbSet<Question> Questions { get; set; }

        public WebQuizDbContext(
            DbContextOptions<WebQuizDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAdd();

            builder.Entity<Question>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnUpdate();
        }
    }
}