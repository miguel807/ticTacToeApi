using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Commands
{
    public sealed record CreateUserCommand(string Name,string chatRoom):IRequest<User>;
}
