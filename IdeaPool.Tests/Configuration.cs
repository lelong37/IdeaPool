#region

using IdeaPool.Application.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace IdeaPool.Tests
{
    public static class Configuration
    {
        public static IConfigurationRoot GetConfigurationRoot(string outputPath)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", true)
                .Build();

            return configurationRoot;
        }
    }
}