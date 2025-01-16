using System.Security.Claims;
using LibraryApi.Models;
public interface IUserService
{
    Task<User> RegisterAsync(string username, string email, string password);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetUserById(int userId);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetMe(ClaimsPrincipal userIdentity); 
    Task Delete(int userId);


}