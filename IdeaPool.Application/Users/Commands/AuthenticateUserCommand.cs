using System;
using System.Collections.Generic;
using System.Text;
using IdeaPool.Domain.Models;
using MediatR;

namespace IdeaPool.Application.Users.Commands
{
    public class AuthenticateUserCommand: IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
