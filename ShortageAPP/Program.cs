using ShortageAPP.UI;

namespace ShortageAPP;

internal abstract class Program
{
    private static void Main()
    {
        while (true)
        {
            StartScreen.ShowAuthenticationScreen();
            Console.Clear();
        }
    }
}