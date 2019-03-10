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
    [Route("api/users")]
    public class UsersController: BaseController
    {
        public UsersController(IdeaPoolContext dataContext)
            : base(dataContext) {}

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> Get()
            => Ok(await Mediator.Send(new GetUsersQuery()));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserCommand command, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(command, cancellationToken));
    }
}