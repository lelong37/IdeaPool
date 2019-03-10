#region

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdeaPool.Api;
using IdeaPool.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    public class UserIntegrationTests
    {
        private const string First = "Long";
        private const string Last = "Le";
        private const string Email = "lelong37@gmail.com";
        private const string Password = "ideaPool@2019";

        [Fact]
        public async Task ShouldRegisterUserAndResponseWithTokenAndRefresh()
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
                First,
                Last,
                Email,
                Password
            });

            var registerContent = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            const string url = @"/api/users";
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = registerContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<User>(responseString);

            // Assert
            responseString.ShouldNotBe(string.Empty);
            user.First.ShouldBe(First);
            user.Last.ShouldBe(Last);
            user.Email.ShouldBe(Email);
            user.Hash.ShouldBeNull();
            user.Salt.ShouldBeNull();
            user.Token.ShouldNotBeNull();
            user.Token.ShouldNotBeEmpty();
            user.RefreshToken.ShouldNotBeEmpty();
        }
    }
}