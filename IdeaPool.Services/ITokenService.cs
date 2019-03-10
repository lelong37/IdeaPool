using System.Collections.Generic;
using System.Security.Claims;

namespace IdeaPool.Services
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateToken(IEnumerable<Claim> claims);
    }
}