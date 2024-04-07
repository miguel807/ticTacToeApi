using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.API.Domain;

namespace Users.API.Infrastucture
{
    public class PostgresUserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        public PostgresUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserByGameRoom(string gameRoom)
        {
            var user = await _context.User.Where(user => user.GameRoom == gameRoom).FirstOrDefaultAsync();
            if (user is null) throw new  ArgumentException("Not user in that chat rooom");

            return user;
        }

        public async  Task<User> GetUserById(Guid id)
        {
            var user = await _context.User.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user is null) throw new ArgumentException("Not user in that chat rooom");

            return user;

        }
    }

}
