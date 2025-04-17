namespace ShortageAPP.Helper;

public static class DataReadHelper
{
    public static string Validate(string? data) 
    {
        while (string.IsNullOrEmpty(data))
        {
            Console.WriteLine("Invalid entry. Please try again.");
            data = Console.ReadLine();
        }

        return data;
    }
    
    public static TEnum ValidateEnum<TEnum>(string? data) where TEnum : struct
    {
        TEnum result;
        while (!Enum.TryParse(data, true, out result) || !Enum.IsDefined(typeof(TEnum), data))
        {
            Console.WriteLine("Invalid entry. Please try again.");
            data = Console.ReadLine();
        }

        return result;
    }
}