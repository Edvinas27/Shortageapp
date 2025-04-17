using ShortageAPP.Helper;
using ShortageAPP.Models;
using ShortageAPP.Services;

namespace ShortageAPP.UI;

public static class ShortageScreen
{
    public static void ShowRegisterShortageScreen(User user)
    {
        Console.WriteLine("Enter the Title of the shortage:");
        var title = Console.ReadLine();
        title = DataReadHelper.Validate(title);
        
        Console.WriteLine("Enter the Name of the shortage:");
        var name = Console.ReadLine();
        name = DataReadHelper.Validate(name);
        
        Console.WriteLine("Enter the Room of the shortage (MeetingRoom,Kitchen,Bathroom):");
        var room = Console.ReadLine();
        var roomEnum = DataReadHelper.ValidateEnum<Room>(room);
        
        Console.WriteLine("Enter the Category of the shortage (Electronics,Food,Other):");
        var category = Console.ReadLine();
        var categoryEnum = DataReadHelper.ValidateEnum<Category>(category);
        
        Console.WriteLine("Enter the Priority of the shortage (1-10):");
        int priority = int.Parse(Console.ReadLine() ?? string.Empty);
        
        while (priority < 1 || priority > 10)
        {
            Console.WriteLine("Priority must be between 1 and 10. Please try again.");
            priority = int.Parse(Console.ReadLine() ?? string.Empty);
        }
        
        var shortage = new Shortage
        {
            Id = Guid.NewGuid(),
            Title = title,
            Name = name,
            RoomType = roomEnum,
            CategoryType = categoryEnum,
            Priority = priority,
            CreatedBy = user.Id,
        };
        
        ShortageService.RegisterShortage(shortage);
    }

    public static void ShowDeleteShortageScreen(User user)
    {
        var existingShortages = ShortageService.GetShortagesByRole(user);
        
        if(existingShortages.Count == 0)
        {
            Console.WriteLine("No shortages available to delete.");
            return;
        }
        
        foreach (var shortage in existingShortages)
        {
            Console.WriteLine($"ID: {shortage.Id}, Title: {shortage.Title}, Name: {shortage.Name}, Room: {shortage.RoomType}, Category: {shortage.CategoryType}, Priority: {shortage.Priority}");
        }

        var (title, name) = ReadData();
            
        if (user.Role != UserRole.Admin && existingShortages.All(s => s.Title != title || s.Name != name))
        {
            Console.WriteLine($"Shortage with Title {title} and Name {name} is not yours.");
            return;
        }
        ShortageService.DeleteShortage(title, name, user);
    }

    public static void ShowListShortagesScreen(User user)
    {
        var existingShortages = ShortageService.GetShortagesByRole(user);

        if (existingShortages.Count == 0)
        {
            Console.WriteLine("No shortages available.");
            return;
        }

        foreach (var shortage in existingShortages)
        {
            Console.WriteLine($"ID: {shortage.Id}, Title: {shortage.Title}, Name: {shortage.Name}, Room: {shortage.RoomType}, Category: {shortage.CategoryType}, Priority: {shortage.Priority}");
        }

        Console.WriteLine("Choose filters: ");
        Console.WriteLine("1. Filter by Title");
        Console.WriteLine("2. Filted by Date");
        Console.WriteLine("3. Filter by Category");
        Console.WriteLine("4. Filter by Room");
        Console.WriteLine("5. Go back");
        Console.Write("Please select an option (1-5): ");
        string? input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input) || int.Parse(input) < 1 || int.Parse(input) > 5)
        {
            Console.Write("Invalid input. Please select an option (1-5): ");
            input = Console.ReadLine();
        }

        switch (input)
        {
            case "1":
                Console.WriteLine("Enter the Title filter:");
                var title = Console.ReadLine();
                title = DataReadHelper.Validate(title);
                PrintFilteredData(FilterService.FilterByTitle(user,title));
                break;
            case "2":
                Console.WriteLine("Enter the Start Date (yyyy-MM-dd):");
                var startDate = Console.ReadLine();
                startDate = DataReadHelper.Validate(startDate);
                Console.WriteLine("Enter the End Date (yyyy-MM-dd):");
                var endDate = Console.ReadLine();
                endDate = DataReadHelper.Validate(endDate);
                PrintFilteredData(FilterService.FilterByDate(user,startDate,endDate));
                break;
            case "3":
                Console.WriteLine("Enter the Category filter (Electronics, Food, Other):");
                var category = Console.ReadLine();
                var categoryEnum = DataReadHelper.ValidateEnum<Category>(category);
                PrintFilteredData(FilterService.FilterByCategory(user,categoryEnum.ToString()));
                break;
            case "4":
                Console.WriteLine("Enter the Room filter (MeetingRoom, Kitchen, Bathroom):");
                var room = Console.ReadLine();
                var roomEnum = DataReadHelper.ValidateEnum<Room>(room);
                PrintFilteredData(FilterService.FilterByRoom(user,roomEnum.ToString()));
                break;
            case "5":
                return;
            default:
                break;
        }
    }

    private static void PrintFilteredData(IEnumerable<Shortage> input)
    {
        var data = input.ToList();
        
        foreach(var shortage in data)
        {
            Console.WriteLine($"ID: {shortage.Id}, Title: {shortage.Title}, Name: {shortage.Name}, Room: {shortage.RoomType}, Category: {shortage.CategoryType}, Priority: {shortage.Priority}");
        }
    }
    private static (string Title, string Name) ReadData()
    {
        Console.WriteLine("Enter the Title and the Name of the shortage you want to delete:");
        Console.Write("Title of the shortage: ");
        var title = Console.ReadLine();
        title = DataReadHelper.Validate(title);
        
        
        Console.Write("Name of the shortage: ");
        var name = Console.ReadLine();
        name = DataReadHelper.Validate(name);

        return (title, name);
    }
}