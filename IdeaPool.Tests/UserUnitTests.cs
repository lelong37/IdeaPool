#region

using System.Threading.Tasks;
using IdeaPool.Application.Users.Commands;
using IdeaPool.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    [Collection(nameof(IdeaPoolContext))]
    public class UserUnitTests
    {
        public UserUnitTests(IdeaPoolContextTextFixture fixture)
        {
            fixture.Initialize();

            var services = fixture.ServiceCollection;
            _provider = services.BuildServiceProvider();

            var ideaPoolContext = _provider.GetService<IdeaPoolContext>();
            ideaPoolContext.Database.EnsureCreated();
        }

        private const string Email = "lelong37@gmail.com";
        private const string Password = "ideaPool@2019";

        private readonly ServiceProvider _provider;

        [Fact]
        public async Task ShouldRegisterAndAuthenticateUserUsingCommands()
        {
            // Arrange (Sign-up, create user)
            var command = new SignUpUserCommand
            {
                First = "Long",
                Last = "Le",
                Email = Email,
                Password = Password
            };

            var mediator = _provider.GetService<IMediator>();

            // Act
            var user = await mediator.Send(command);

            // Assert
            user.ShouldBeOfType<User>();
            user.Email.ShouldBe(Email);
            user.Token.ShouldNotBeNull();
            user.Token.ShouldNotBeEmpty();

            // Arrange (Login, created user)
            var loginUserCommand = new LoginUserCommand
            {
                Email = user.Email,
                Password = Password
            };

            // Act
            user = await mediator.Send(loginUserCommand);

            /// Assert
            user.ShouldBeOfType<User>();
            user.Email.ShouldBe(Email);
            user.Token.ShouldNotBeNull();
            user.Token.ShouldNotBeEmpty();

            // Arrange (Refresh Token)
            var refreshTokenCommand = new RefreshTokenCommand
            {
                Token = user.Token,
                RefreshToken = user.RefreshToken
            };

            var refresh = await mediator.Send(refreshTokenCommand);
            refresh.Token.ShouldNotBeNullOrEmpty();
            refresh.RefreshToken.ShouldNotBeNullOrEmpty();
        }
    }
}