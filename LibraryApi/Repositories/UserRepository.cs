using System.Security.Claims;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class UserRepository : IUserService
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            if (user is not null && user.PasswordHash == password)
            {
                return user;
            }
            return null;
        }
        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<User?> GetMe(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            
            
            if (userIdClaim is null)
            {
                return null;
            }
            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                if (user is not null)
                {
                    var userEntity = _context.Users.SingleOrDefault(x => x.Id == userId);
                }
                return await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            }
            else
            {
                var username = userIdClaim.Value;
                return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            }
        }
        public async Task Delete(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
