using System.Security.Claims;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LibraryApi.Repositories
{
    public class UserRepository : IUserService
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        // Regex pattern for validating a strong password
        // // At least one lowercase letter
        // At least one uppercase letter
        // at least one digit \d
        // at least one special character @#$%^&
        // at least 8 character in length
        //Example password: "Examplepasword0012@!"
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        // Regex pattern for validating an email
        private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        //This regex checks if the email has the format username@domain.com.
        

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            // Validate email format using regex
            if (!Regex.IsMatch(email, EmailRegex))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Validate password format using regex
            if (!Regex.IsMatch(password, PasswordRegex))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.");
            }

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
            var userIdClaim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier && int.TryParse(c.Value, out _));

            Console.WriteLine($"User id claim Value: {userIdClaim?.Value}");

            if (userIdClaim is null)
            {
                return null;
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
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
