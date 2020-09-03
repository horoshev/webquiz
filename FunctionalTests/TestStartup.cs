using System.Data.Common;
using Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web;
using Config = Web.IdentityServerConfiguration;

namespace FunctionalTests
{
    public sealed class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<WebQuizDbContext>(builder =>
            {
                builder.UseSqlite(CreateInMemoryDatabase());
            });
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("DataSource=:memory:"); //"Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}