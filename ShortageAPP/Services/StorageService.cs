using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShortageAPP.Services;

public static class StorageService<T>
{
    public static IEnumerable<T> ReadStorage(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
               File.WriteAllText(filePath, "[]");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<IEnumerable<T>>(json, options) ?? [];

        }
        catch (Exception e)
        {
            Console.WriteLine($"File {filePath} not found. Creating file...");
            return [];
        }
    }
    
    public static void WriteToStorage(string filePath, IEnumerable<T> data)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            string json;
            
            if (filePath == "shortages.json")
            {
                IEnumerable<T> orderedData = data.OrderByDescending(d => d.GetType().GetProperty("Priority").GetValue(d, null));
                json = JsonSerializer.Serialize(orderedData, options);
                File.WriteAllText(filePath, json);
                return;
            }
            
            json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not write to storage: {e.Message}");
        }
    }
}