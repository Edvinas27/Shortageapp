using ShortageAPP.Models;

namespace ShortageAPP.Services;

public static class UserService
{
    private const string FilePath = "users.json";

    public static User? LoginUser(string name, string password)
    {
        var users = StorageService<User>.ReadStorage(FilePath);
        
        foreach (var user in users)
        {
            if (!user.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || user.Password != password) continue;
            
            return user;
        }
        Console.WriteLine("Invalid username or password.");
        return null;
    }

    public static bool RegisterUser(string name, string password, UserRole role)
    {
        var users = StorageService<User>.ReadStorage(FilePath).ToList();
        
        if(users.Any(user => user.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }
        
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Password = password,
            Role = role
        };
        
        users.Add(newUser);
        StorageService<User>.WriteToStorage(FilePath, users);
        return true;
    }
}