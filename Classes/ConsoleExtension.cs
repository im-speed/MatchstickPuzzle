namespace MatchstickPuzzle.Classes;
internal static class ConsoleExtension
{
    public static void WriteColoredLine(object value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}
