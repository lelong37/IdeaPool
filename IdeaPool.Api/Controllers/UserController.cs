#region

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Application.Users.Commands;
using IdeaPool.Application.Users.Queries;
using IdeaPool.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace IdeaPool.Api.Controllers
{
    public class UserController: BaseController
    {
        public UserController(IdeaPoolContext dataContext)
            : base(dataContext) {}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await Mediator.Send(command, cancellationToken);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.First,
                user.Last,
                user.Token
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> Get()
            => Ok(await Mediator.Send(new GetUsersQuery()));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(command, cancellationToken));
    }
}