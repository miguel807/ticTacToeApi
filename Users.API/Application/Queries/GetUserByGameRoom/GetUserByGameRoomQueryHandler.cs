using MediatR;
using Users.API.Domain;

namespace Users.API.Application.Queries.GetUserByGameRoom
{
    internal sealed class GetUserByGameRoomQueryHandler : IRequestHandler<GetUserByGameRoomQuery, User>
    {

        private readonly IUserRepository _userRepository;

        public GetUserByGameRoomQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        async  Task<User> IRequestHandler<GetUserByGameRoomQuery, User>.Handle(GetUserByGameRoomQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByGameRoom(request.gameRoom);
            return user;

        }
    }
}
