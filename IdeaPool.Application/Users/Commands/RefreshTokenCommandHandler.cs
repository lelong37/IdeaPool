#region

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Application.Exceptions;
using IdeaPool.Application.Users.Models;
using IdeaPool.Domain.Infrastructure;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class RefreshTokenCommandHandler: IRequestHandler<RefreshTokenCommand, Refresh>
    {
        private readonly AppSettings _appSettings;
        private readonly IdeaPoolContext _dataContext;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(IdeaPoolContext dataContext, IUserService userService, ITokenService tokenService, IOptions<AppSettings> appSettings)
        {
            _dataContext = dataContext;
            _userService = userService;
            _tokenService = tokenService;
            _appSettings = appSettings.Value;
        }

        public async Task<Refresh> Handle(RefreshTokenCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            var username = principal.Identity.Name;

            var user = await _dataContext.User.SingleOrDefaultAsync(x => x.Email == username, cancellationToken);

            if (user.RefreshToken != request.RefreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = _tokenService.GenerateToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _dataContext.SaveChangesAsync(cancellationToken);

            return new Refresh
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}