using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Commands
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


       async  Task<User> IRequestHandler<CreateUserCommand, User>.Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User(request.Name, request.chatRoom);
            var userCreated = await _userRepository.CreateUserAsync(newUser);
            return userCreated;
        }
    }
}
