using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.API.Application.Commands;
using Users.API.Application.Queries.GetAllUser;
using Users.API.Application.Queries.GetUserByGameRoom;
using Users.API.Application.Queries.GetUserById;
using Users.API.Domain;

namespace Users.API.Infrastucture
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public UserController(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator; 
        }

        [HttpPost]
        public async Task<ActionResult<User>> createUSer([FromBody] User user)
        {
            var command = new CreateUserCommand(user.Name, user.GameRoom);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{gameRoom}")]
        public async Task<ActionResult<User>> GetUserByGameRoom(string gameRoom)
        {
            var query = new GetUserByGameRoomQuery(gameRoom);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var query = new GetAllUserQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
