#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Application.Exceptions;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class SignUpUserCommandHandler: IRequestHandler<SignUpUserCommand, User>
    {
        private readonly IdeaPoolContext _dataContext;
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public SignUpUserCommandHandler(IdeaPoolContext dataContext, IUserService userService, IMediator mediator)
        {
            _dataContext = dataContext;
            _userService = userService;
            _mediator = mediator;
        }

        public async Task<User> Handle(SignUpUserCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(string.IsNullOrWhiteSpace(command.Password))
                throw new AppException("Password is required");

            if(_dataContext.User.Any(x => x.Email == command.Email))
                throw new AppException("Username \"" + command.Email + "\" is already taken");

            _userService.CreatePasswordHash(command.Password, out var passwordHash, out var passwordSalt);

            var user = new User
            {
                First = command.First,
                Last = command.Last,
                Email = command.Email,
                Hash = passwordHash,
                Salt = passwordSalt
            };

            _dataContext.User.Add(user);
            
            await _dataContext.SaveChangesAsync(cancellationToken);

            var loginUserCommand = new LoginUserCommand
            {
                Email = command.Email,
                Password = command.Password,
            };

            user = await _mediator.Send(loginUserCommand, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}