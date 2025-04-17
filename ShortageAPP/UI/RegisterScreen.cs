using ShortageAPP.Helper;
using ShortageAPP.Models;
using ShortageAPP.Services;

namespace ShortageAPP.UI;

public static class RegisterScreen
{
    public static void ShowRegisterScreen()
    {
        Console.WriteLine("Welcome to the Registration Screen");
        Console.WriteLine("Please enter your name:");
        var name = Console.ReadLine();
        name = DataReadHelper.Validate(name);
        
        Console.WriteLine("Please enter your password:");
        var password = Console.ReadLine();
        password = DataReadHelper.Validate(password);
        
        
        Console.WriteLine("Please select your role (Admin/User):");
        var roleInput = Console.ReadLine();
        var role = DataReadHelper.ValidateEnum<UserRole>(roleInput);

        if (UserService.RegisterUser(name, password, role))
        {
            Console.WriteLine($"User {name} registered successfully.");
        }
    }
}