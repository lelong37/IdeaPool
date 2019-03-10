#region

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Application.Exceptions;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class LoginUserCommandHandler: IRequestHandler<LoginUserCommand, User>
    {
        private readonly IdeaPoolContext _dataContext;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IdeaPoolContext dataContext, IUserService userService, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return null;

            var user = await _dataContext
                .User
                .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if(user == null)
                throw new AuthenticationException("Username or password is incorrect");

            if(!_userService.VerifyPasswordHash(request.Password, user.Hash, user.Salt))
                throw new AuthenticationException("Username or password is incorrect");

            user.Token = _tokenService.GenerateToken(new[] { new Claim(ClaimTypes.Name, user.Email) });
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            return user;
        }
    }
}