using MatchstickPuzzle.Classes.Menu.CloseActions;
using MatchstickPuzzle.Classes.Menu.Styles;

namespace MatchstickPuzzle.Classes.Menu;

/// <summary>
/// A menu that displays the menu options in a vertical list.
/// </summary>
internal class ListMenu : IMenu
{
    public bool Opened { get; set; }
    public List<Option> Options { get; }
    public string? Message { get; set; }
    public ICloseAction CloseAction { get; set; } = new StandardClose();
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;
    public IOptionStyle OptionStyle { get; set; } = new DefaultStyle("  ", "> ", "", "");

    private int _selected;
    private int Selected
    {
        get => _selected;
        set
        {
            if (0 <= value && value < Options.Count)
            {
                _selected = value;
            }
        }
    }

    public ListMenu(List<Option> options)
    {
        Options = options;
    }

    private void WriteOptions()
    {
        Console.Clear();
        if (Message != null)
        {
            ConsoleExtension.WriteColoredLine(Message, ConsoleColor.Cyan);
        }

        for (int i = 0; i < Options.Count; i++)
        {
            Console.WriteLine(OptionStyle.Style(Options[i], i == Selected));
        }
    }

    public void Open()
    {
        if (Options.Count < 1)
        {
            throw new NullReferenceException("Can not opan a menu with no options.");
        }

        Opened = true;
        while (Opened)
        {
            WriteOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                if (CloseWithEscape)
                    CloseAction.Close(this);
            }
            else if (keyInfo.Key == ConsoleKey.Enter
                || keyInfo.Key == ConsoleKey.Spacebar)
            {
                Options[Selected].Action();
                if (CloseAfterAction)
                    CloseAction.QuickClose(this);
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow
                || keyInfo.Key == ConsoleKey.W)
            {
                Selected--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow
                || keyInfo.Key == ConsoleKey.S)
            {
                Selected++;
            }
        }
    }

    /// <summary>
    /// Creates a temporary list menu to ask the user a yes or no question.
    /// </summary>
    /// <param name="message">What to ask the user.</param>
    /// <returns>True if they said yes, false if no.</returns>
    public static bool YesOrNo(string message, bool defaultToNo = false)
    {
        bool answer = false;
        List<Option> options = new()
        {
            new("Yes", () => answer = true),
            new("No", () => answer = false)
        };

        ListMenu yesOrNo = new(options)
        {
            Message = message,
            CloseWithEscape = true
        };

        if (defaultToNo)
        {
            yesOrNo.Selected = 1;
        }

        yesOrNo.Open();

        return answer;
    }
}
