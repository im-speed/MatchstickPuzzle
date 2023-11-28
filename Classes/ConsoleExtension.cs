namespace MatchstickPuzzle.Classes;
/// <summary>
/// A class with console related helper functions.
/// </summary>
internal static class ConsoleExtension
{
    /// <summary>
    /// Writes an object to the console in the specified color and resets the color afterwards.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="color">Color to write in.</param>
    public static void WriteColoredLine(object? value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}
