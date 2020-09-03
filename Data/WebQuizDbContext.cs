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
        public WebQuizDbContext(
            DbContextOptions<WebQuizDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .Property(b => b.CreatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAdd();

            builder.Entity<Question>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}