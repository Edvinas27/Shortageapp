using System.Runtime.InteropServices.JavaScript;
using ShortageAPP.Services;

namespace ShortageAPP.Models;


public enum Room
{
    MeetingRoom,
    Kitchen,
    Bathroom
}

public enum Category
{
    Electronics,
    Food,
    Other
}

public class Shortage
{
    public required Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Title { get; set; } = string.Empty;
    public required Room RoomType { get; set; }
    public required Category CategoryType { get; set; }
    public required int Priority { get; set; }
    public  DateTime CreatedAt { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
    public required Guid CreatedBy { get; set; }
}