using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetAllUser
{
    public sealed record GetAllUserQuery:IRequest<IEnumerable<User>>;
   
}
