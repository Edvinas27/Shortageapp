using ShortageAPP.Helper;
using ShortageAPP.Models;

namespace ShortageAPP.Services;

public static class FilterService
{
     public static IEnumerable<Shortage> FilterByTitle(User user, string title)
    {
        var existingShortages = ShortageService.GetShortagesByRole(user);
        var filteredShortages = existingShortages.Where(s => s.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredShortages.Count == 0)
        {
            Console.WriteLine($"No shortages found with Title {title}.");
            return [];
        }
        
        return filteredShortages;
    }

    public static IEnumerable<Shortage> FilterByDate(User user, string startDate, string endDate)
    {
        if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
        {
            Console.WriteLine("Invalid date format. Please try again.");
            return [];
        }
        
        var existingShortages = ShortageService.GetShortagesByRole(user);
        var filteredShortages = existingShortages.Where(s => s.CreatedAt >= start && s.CreatedAt <= end).ToList();
        
        if (filteredShortages.Count == 0)
        {
            Console.WriteLine($"No shortages found between {start.ToShortDateString()} and {end.ToShortDateString()}.");
            return [];
        }
        
        return filteredShortages;
    }

    public static IEnumerable<Shortage> FilterByCategory(User user, string category)
    {
        var existingShortages = ShortageService.GetShortagesByRole(user);
        var filteredShortages = existingShortages.Where(s => s.CategoryType.ToString().Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredShortages.Count == 0)
        {
            Console.WriteLine($"No shortages found with Category {category}.");
            return [];
        }
        
        return filteredShortages;
    }
    
    public static IEnumerable<Shortage> FilterByRoom(User user, string room)
    {
        var existingShortages = ShortageService.GetShortagesByRole(user);
        var filteredShortages = existingShortages.Where(s => s.RoomType.ToString().Equals(room, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (filteredShortages.Count == 0)
        {
            Console.WriteLine($"No shortages found with Room {room}.");
            return [];
        }
        
        return filteredShortages;
    }
}