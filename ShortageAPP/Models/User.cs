using ShortageAPP.Services;

namespace ShortageAPP.Models;

public enum UserRole
{
    Admin,
    User
}

public class User
{
    public required Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required UserRole Role { get; set; }
}