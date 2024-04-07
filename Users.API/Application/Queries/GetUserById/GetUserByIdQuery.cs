using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid id):IRequest<User>;
}
