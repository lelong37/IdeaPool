#region

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace IdeaPool.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController: ControllerBase
    {
        private IMediator _mediator;

        protected BaseController(DbContext dataContext) 
            => dataContext.Database.EnsureCreated();

        protected IMediator Mediator 
            => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}