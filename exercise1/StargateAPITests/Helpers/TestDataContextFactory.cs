using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StargateAPITests.Helpers
{
    public class TestDataContextFactory
    {
        public TestDataContextFactory()
        {
            var builder = new DbContextOptionsBuilder<StargateContext>();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            builder.UseSqlite(connection);

            using (var ctx = new StargateContext(builder.Options))
            {
                ctx.Database.EnsureCreated();
            }

            _options = builder.Options;
        }

        private readonly DbContextOptions<StargateContext> _options;

        public StargateContext Create() => new StargateContext(_options);
    }
}
