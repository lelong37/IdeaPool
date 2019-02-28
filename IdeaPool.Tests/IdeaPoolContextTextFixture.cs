#region

using System;
using System.IO;
using IdeaPool.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    [Collection(nameof(IdeaPoolContext))]
    public class IdeaPoolContextTextFixture
    {
        private IdeaPoolContext _context;

        public IdeaPoolContext Context
        {
            get
            {
                if(_context == null)
                    throw new InvalidOperationException("You must first call Initialize before getting the context.");
                return _context;
            }
        }

        public void Initialize(bool useInMemory = true, Action seedData = null)
        {
            var configuration = Configuration.GetConfigurationRoot(Directory.GetCurrentDirectory());

            DbContextOptions<IdeaPoolContext> options;

            if(useInMemory)
            {
                var connection = new SqliteConnection(configuration.GetConnectionString("IdeaPoolContextInMemory"));
                connection.Open();

                options = new DbContextOptionsBuilder<IdeaPoolContext>()
                    .UseSqlite(connection)
                    .Options;
            }
            else
            {
                options = new DbContextOptionsBuilder<IdeaPoolContext>()
                    .UseSqlServer(configuration.GetConnectionString("IdeaPoolContextSqlServer"))
                    .Options;
            }

            _context = new IdeaPoolContext(options);
            _context.Database.EnsureCreated();
            seedData?.Invoke();
        }
    }
}