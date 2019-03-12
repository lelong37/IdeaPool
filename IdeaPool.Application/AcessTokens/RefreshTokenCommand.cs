#region

using IdeaPool.Application.Users.Models;
using MediatR;

#endregion

namespace IdeaPool.Application.AcessTokens
{
    public class RefreshTokenCommand: IRequest<Refresh>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}