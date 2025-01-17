using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Username{ get; set; }

    [Required]
    public string? PasswordHash{ get; set; }

    [Required]
    public string? Email{ get; set; }

    [Required]
    public UserRole Role { get; set; }

    public User()
    {
    }

    public User(int id, string username, string passwordHash, string email)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        Role = UserRole.NormalUser;
    }
}

public enum UserRole
    {
        NormalUser,
        SuperUser
    }
}