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

#endregion

namespace IdeaPool.Application.Users.Commands
{
    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, User>
    {
        private readonly IdeaPoolContext _dataContext;
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IdeaPoolContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }

        public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(string.IsNullOrWhiteSpace(command.Password))
                throw new AppException("Password is required");

            if(_dataContext.User.Any(x => x.Email == command.Email))
                throw new AppException("Username \"" + command.Email + "\" is already taken");

            _userService.CreatePasswordHash(command.Password, out var passwordHash, out var passwordSalt);

            var user = new User
            {
                Email = command.Email,
                Hash = passwordHash,
                Salt = passwordSalt
            };

            _dataContext.User.Add(user);

            try
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return user;
        }
    }
}