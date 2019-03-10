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
    [Route("access-tokens")]
    public class AccessTokens: BaseController
    {
        public AccessTokens(IdeaPoolContext dataContext)
            : base(dataContext) {}

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> Get()
            => Ok(await Mediator.Send(new GetUsersQuery()));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
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

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
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
    }
}