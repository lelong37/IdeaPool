#region

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdeaPool.Api;
using IdeaPool.Application.Users.Commands;
using IdeaPool.Domain.Infrastructure;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    [Collection(nameof(IdeaPoolContext))]
    public class UserTest
    {
        public UserTest(IdeaPoolContextTextFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize(true);
        }

        private const string Email = "lelong37@gmail.com";
        private const string Password = "ideaPool@2019";

        private readonly IdeaPoolContextTextFixture _fixture;

        [Fact]
        public async Task Register()
        {
            // Arrange
            const string projectDirectory = @"C:\Sources\IdeaPool\IdeaPool.Api";

            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDirectory)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>();

            var testServer = new TestServer(builder);
            var httpClient = testServer.CreateClient();

            var jsonRegister = JsonConvert.SerializeObject(new
            {
                Email,
                Password
            });

            var registerContent = new StringContent(jsonRegister, Encoding.UTF8, "application/json");

            string url = @"/api/user/register";

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = registerContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            responseString.ShouldNotBe(string.Empty);
        }

        [Fact]
        public async Task RegisterAndAuthenticate()
        {
            // Arrange
            var dataContext = _fixture.Context;
            var userService = new UserService();
            var configuration = Configuration.GetConfigurationRoot(Directory.GetCurrentDirectory());
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var optionsWrapper = new OptionsWrapper<AppSettings>(appSettings);

            // Act
            var createUserCommandHandler = new CreateUserCommandHandler(dataContext, userService);

            var result = await createUserCommandHandler.Handle(new CreateUserCommand
            {
                Email = Email,
                First = "Long",
                Last = "Le",
                Password = Password
            });

            // Assert
            result.ShouldBeOfType<User>();
            result.Email.ShouldBe(Email);

            // Arrange
            var authenticateUserHandler = new AuthenticateUserCommandHandler(dataContext, userService, optionsWrapper);

            // Act
            var authenticateResult = await authenticateUserHandler.Handle(new AuthenticateUserCommand
            {
                Email = Email,
                Password = Password
            });

            /// Assert
            authenticateResult.ShouldBeOfType<User>();
            result.Email.ShouldBe(Email);
        }
    }
}