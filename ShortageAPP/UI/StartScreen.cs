using ShortageAPP.Helper;
using ShortageAPP.Models;

namespace ShortageAPP.UI;

public static class StartScreen
{
    public static void ShowAuthenticationScreen()
    {
        while (true)
        {
            Console.Clear();
            DisplayAuthenticationMenu();
            string? input = Console.ReadLine();
            DataReadHelper.Validate(input);
            
            switch (input)
            {
                case "1":
                    Console.Clear();
                    var user = LoginScreen.ShowLoginScreen();
                    if (user is not null)
                    {
                        ShowMainScreen(user);
                    }
                    break;
                case "2":
                    Console.Clear();
                    RegisterScreen.ShowRegisterScreen();
                    break;
                case "3":
                    Console.WriteLine("Exiting the application. Goodbye!");
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private static void ShowMainScreen(User user)
    {
        while (true)
        {
            DisplayMainMenu(user);
            string? input = Console.ReadLine();
            input = DataReadHelper.Validate(input);
            
            switch (input)
            {
                case "1":
                    Console.Clear();
                    ShortageScreen.ShowRegisterShortageScreen(user);
                    break;
                case "2":
                    Console.Clear();
                    ShortageScreen.ShowDeleteShortageScreen(user);
                    break;
                case "3":
                    Console.Clear();
                    ShortageScreen.ShowListShortagesScreen(user);
                    break;
                case "4":
                    Console.Clear();
                    return;
                case "5":
                    Console.WriteLine("Exiting the application. Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }
    private static void DisplayAuthenticationMenu()
    {
        Console.WriteLine("Welcome to the Shortage Management System");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        Console.Write("Please select an option (1-3): ");
    }

    private static void DisplayMainMenu(User user)
    {
        Console.WriteLine($"Welcome {user.Name} to the Shortage Management System");
        Console.WriteLine($"You are logged in as {user.Role}");
        Console.WriteLine("1. Register a new shortage");
        Console.WriteLine("2. Delete a shortage");
        Console.WriteLine("3. List shortages");
        Console.WriteLine("4. Log Out");
        Console.WriteLine("5. Exit");
        Console.Write("Please select an option (1-5): ");
    }
}