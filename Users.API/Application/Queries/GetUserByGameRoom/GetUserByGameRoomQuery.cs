using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetUserByGameRoom
{
    public sealed record GetUserByGameRoomQuery(string gameRoom):IRequest<User>;
}
