using ShortageAPP.Models;

namespace ShortageAPP.Services;

public static class ShortageService
{
    private const string FilePath = "shortages.json";
    public static bool RegisterShortage(Shortage shortage)
    {
        var existingShortages = StorageService<Shortage>.ReadStorage(FilePath).ToList();

        if (existingShortages.Any(data => shortage.Name.ToLower() == data.Name.ToLower() && shortage.Title.ToLower() == data.Title.ToLower() && shortage.Priority > data.Priority))
        {
            Console.WriteLine("Overriding existing shortage. Given shortage has a higher priority.");
            var overrideShortage = existingShortages.First(data => shortage.Name.ToLower() == data.Name.ToLower() && shortage.Title.ToLower() == data.Title.ToLower());
            overrideShortage.Priority = shortage.Priority;
            overrideShortage.CreatedAt = shortage.CreatedAt;
            overrideShortage.CreatedBy = shortage.CreatedBy;
            overrideShortage.RoomType = shortage.RoomType;
            overrideShortage.CategoryType = shortage.CategoryType;
            StorageService<Shortage>.WriteToStorage(FilePath, existingShortages);
            return true;
        }
        
        if (existingShortages.Any(data => shortage.Name.ToLower() == data.Name.ToLower() && shortage.Title.ToLower() == data.Title.ToLower()))
        {
            Console.WriteLine("Shortage already exists with the same name and title.");
            return false;
        }
        Console.WriteLine($"Shortage '{shortage.Name}' registered successfully.");
        existingShortages.Add(shortage);
        StorageService<Shortage>.WriteToStorage(FilePath, existingShortages);
        return true;
    }

    public static void DeleteShortage(string title, string name, User user)
    {
        var existingShortages = GetShortagesByRole(user);
        
        var shortageToDelete = existingShortages.FirstOrDefault(s => s.Title.ToLower() == title.ToLower() && s.Name.ToLower() == name.ToLower());

        if (shortageToDelete is null)
        {
            Console.WriteLine($"Shortage with Title {title} and Name {name} not found.");
            return;
        }
        
        existingShortages.Remove(shortageToDelete);
        StorageService<Shortage>.WriteToStorage(FilePath, existingShortages);
        Console.WriteLine($"Shortage with Title {title} and Name {name} deleted successfully.");
    }
    
    public static List<Shortage> GetShortagesByRole(User user)
    {
        var shortages = StorageService<Shortage>.ReadStorage("shortages.json").ToList();
        return user.Role == UserRole.Admin ? shortages : shortages.Where(s => s.CreatedBy == user.Id).ToList();
    }
}