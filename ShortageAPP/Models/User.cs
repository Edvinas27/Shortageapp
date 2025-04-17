using ShortageAPP.Services;

namespace ShortageAPP.Models;

public enum UserRole
{
    Admin,
    User
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}