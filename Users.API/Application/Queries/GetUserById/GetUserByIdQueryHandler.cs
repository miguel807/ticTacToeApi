using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetUserById
{
    internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {

        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async  Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(request.id);
            return user;
        }
    }
}
