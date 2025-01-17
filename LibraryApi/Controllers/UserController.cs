using Microsoft.AspNetCore.Mvc;
using LibraryApi.DTOs;
using LibraryApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public UserController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }
    /// <summary>
    /// Get list of users
    /// </summary>
    /// <returns>List of Users</returns>
    [HttpGet("Browse")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers(); // Await the async method
        return Ok(users);
    }
    /// <summary>
    /// Get User data by its id.
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User data by given id.</returns>
    [HttpGet("SearchById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserById(id); // Await the async method
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="registerDto">DTO from body</param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            var user = await _userService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            // Return a BadRequest response with a structured error message
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    /// <summary>
    /// Get logged in user data
    /// </summary>
    /// <returns>Current user data</returns>
    [HttpGet("GetMe")]
    public async Task<IActionResult> GetMe()
    {
        var currentUser = await _userService.GetMe(User); // Await the async method
        if (currentUser == null)
        {
            return Unauthorized(new { message = "User not authenticated." });
        }

        return Ok(currentUser);
    }
    /// <summary>
    /// Authorize user
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password); // Await the async method
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
    /// <summary>
    /// Remove user from Db by id
    /// </summary>
    /// <param name="userId">Specific user Id</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var currentUser = await _userService.GetMe(User);

        if (currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        var deletedUser = _userService.GetUserById(userId);
        if (deletedUser is null)
        {
            return NotFound();
        }

        _userService.Delete(userId);
        return NoContent();
    }
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            // new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
