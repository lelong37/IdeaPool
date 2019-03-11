#region

using IdeaPool.Domain.Models;
using MediatR;

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class SignUpUserCommand: IRequest<User>
    {
        public string Email { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Password { get; set; }
    }
}