using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetAllUser
{
    internal sealed class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IEnumerable<User>>
    {

        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAllUser();
            return user;
        }
    }
}
