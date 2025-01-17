// using System.Security.Claims;
// using LibraryApi.Models;
// public class InMemoryUserService : IUserService{
//     private readonly List<User> _users = new List<User>();
//     private int _nextId = 1;


//     public InMemoryUserService()
//     {
//         _users.Add(new User {
//             Id = _nextId++,
//             Username = "testuser",
//             Email = "test@test.com",
//             PasswordHash = "password",
//             Role = UserRole.SuperUser
//             });
//         _users.Add(new User {
//             Id =_nextId++,
//             Username = "string1",
//             Email = "test@test.com",
//             PasswordHash =  "string",
//             Role = UserRole.SuperUser
//             });
//     }
//     public Task<User> RegisterAsync(string username, string email, string password){
//         var user = new User{
//             Id = _nextId++,
//             Username = username,
//             Email = email,
//             PasswordHash = password
//         };
//         _users.Add(user);
//         return Task.FromResult(user);
//     }

//     public IEnumerable<User> GetAllUsers() => _users;
//     public Task<User?> AuthenticateAsync(string username, string password)
//     {
//         var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
//         if (user != null && user.PasswordHash == password)
//         {
//             return Task.FromResult(user);
//         }
//         return Task.FromResult<User?>(null);
//     }
//     public User? GetUserById(int userId)
//     {
//         return _users.FirstOrDefault(u => u.Id == userId);
//     }
//     public User GetMe(ClaimsPrincipal user)
//     {
//         var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
//         if (userIdClaim == null)
//         {
//             return null;
//         }
//         if (int.TryParse(userIdClaim.Value, out int userId))
//         {
//             return _users.SingleOrDefault(x => x.Id == userId);
//         }
//         else
//         {
//             return _users.SingleOrDefault(x => x.Username == userIdClaim.Value);
//         }
//     }
// }