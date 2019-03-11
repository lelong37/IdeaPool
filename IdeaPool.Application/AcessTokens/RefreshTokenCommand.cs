#region

using IdeaPool.Application.Users.Models;
using IdeaPool.Domain.Models;
using MediatR;

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class RefreshTokenCommand: IRequest<Refresh>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}