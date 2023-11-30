using MatchstickPuzzle.Classes.Menu.CloseActions;

namespace MatchstickPuzzle.Classes.Menu;

/// <summary>
/// A menu that displays the menu options in a vertical list.
/// </summary>
internal class ListMenu : IMenu
{
    public bool Opened { get; set; }
    public List<MenuOption> Options { get; }
    public string? Message { get; set; }
    public ICloseAction CloseAction { get; set; } = new StandardClose();
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;

    /// <summary>
    /// Default: empty
    /// </summary>
    public string OptionPrefix { get; set; } = " ";

    /// <summary>
    /// Default: >
    /// </summary>
    public string SelectedPrefix { get; set; } = ">";

    /// <summary>
    /// Default: empty
    /// </summary>
    public string OptionSuffix { get; set; } = "";

    /// <summary>
    /// Default: empty
    /// </summary>
    public string SelectedSuffix { get; set; } = "";

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

    public ListMenu(List<MenuOption> options)
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
            Console.WriteLine($"{(i == Selected ? SelectedPrefix : OptionPrefix)} {Options[i].Text}");
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
                    CloseAction.Close(this);
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
    public static bool YesOrNo(string message)
    {
        bool answer = false;
        List<MenuOption> options = new()
        {
            new("Yes", () => answer = true),
            new("No", () => answer = false)
        };

        ListMenu yesOrNo = new(options)
        {
            Message = message,
            CloseWithEscape = true
        };

        yesOrNo.Open();

        return answer;
    }
}
