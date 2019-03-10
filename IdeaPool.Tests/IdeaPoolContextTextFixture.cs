#region

using System;
using System.IO;
using System.Reflection;
using IdeaPool.Application.Infrastructure;
using IdeaPool.Application.Users.Commands;
using IdeaPool.Domain.Infrastructure;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    [Collection(nameof(IdeaPoolContext))]
    public class IdeaPoolContextTextFixture
    {
        public ServiceCollection ServiceCollection
        {
            get
            {
                if (_services == null)
                    throw new InvalidOperationException("You must first call Initialize before getting the context.");
                return _services;
            }
        }

        private ServiceCollection _services;

        public void Initialize(bool useInMemory = true, Action seedData = null)
        {
            _services = new ServiceCollection();

            var configuration = Configuration.GetConfigurationRoot(Directory.GetCurrentDirectory());

            var appSettingsSection = configuration.GetSection("AppSettings");
            var settings = appSettingsSection.Get<AppSettings>();

            _services.Configure<AppSettings>(appSettingsSection);

            _services.AddDbContext<IdeaPoolContext>(options =>
            {
                string connectionString;

                if (!settings.UseInMemoryDatabase.GetValueOrDefault(false))
                {
                    connectionString = configuration.GetConnectionString("IdeaPoolContextSql");
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    connectionString = configuration.GetConnectionString("IdeaPoolContextInMemory");

                    var connection = new SqliteConnection(connectionString);
                    connection.Open();

                    options.UseSqlite(connection);
                }

                seedData?.Invoke();
            });

            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<ITokenService, TokenService>();
            _services.AddMediatR(typeof(SignUpUserCommand).GetTypeInfo().Assembly);
            _services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            _services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        }
    }
}