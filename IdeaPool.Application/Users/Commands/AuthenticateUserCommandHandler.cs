using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Application.Exceptions;
using IdeaPool.Application.Infrastructure;
using IdeaPool.Domain.Infrastructure;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdeaPool.Application.Users.Commands
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, User>
    {
        private readonly IdeaPoolContext _dataContext;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public AuthenticateUserCommandHandler(IdeaPoolContext dataContext, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _dataContext = dataContext;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        public async Task<User> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                _dataContext.Database.EnsureCreated();

                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return null;

                var userFromDb = await _dataContext
                    .User
                    .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

                if (userFromDb == null)
                    throw new AuthenticationException("Username or password is incorrect");

                if (!_userService.VerifyPasswordHash(request.Password, userFromDb.Hash, userFromDb.Salt))
                    throw new AuthenticationException("Username or password is incorrect");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userFromDb.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                userFromDb.Token = tokenString;

                return userFromDb;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
