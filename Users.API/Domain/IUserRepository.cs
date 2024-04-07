
namespace Users.API.Domain
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user);

        public Task<User> GetUserById(Guid id);

        public Task<IEnumerable<User>> GetAllUser();

        public Task<User> GetUserByGameRoom(string gameRoom);
    }

}
