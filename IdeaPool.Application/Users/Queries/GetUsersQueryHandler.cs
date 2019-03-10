#region

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdeaPool.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace IdeaPool.Application.Users.Queries
{
    public class GetUsersQueryHandler: IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly IdeaPoolContext _dataContext;

        public GetUsersQueryHandler(IdeaPoolContext dataContext) 
            => _dataContext = dataContext;

        public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken) 
            => await _dataContext.User.ToListAsync(cancellationToken);
    }
}