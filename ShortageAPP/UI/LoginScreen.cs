using ShortageAPP.Helper;
using ShortageAPP.Models;
using ShortageAPP.Services;

namespace ShortageAPP.UI;

public static class LoginScreen
{
    public static User? ShowLoginScreen()
    {
        Console.WriteLine("Welcome to the Login Screen");
        Console.WriteLine("Please enter your name:");
        var name = Console.ReadLine();
        name = DataReadHelper.Validate(name);

        Console.WriteLine("Please enter your password:");
        string? password = Console.ReadLine();
        password = DataReadHelper.Validate(password);
        
        var user = UserService.LoginUser(name, password);

        if (user is not null)
        {
            Console.WriteLine($"Welcome back {name}!");
            return user;
        }

        return null;
    }
}