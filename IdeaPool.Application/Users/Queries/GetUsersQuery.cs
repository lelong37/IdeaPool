using System;
using System.Collections.Generic;
using System.Text;
using IdeaPool.Domain.Models;
using MediatR;

namespace IdeaPool.Application.Users.Queries
{
    public class GetUsersQuery: IRequest<IEnumerable<User>>
    {
    }
}
