using MatchstickPuzzle.Classes.Menu.CloseActions;
using MatchstickPuzzle.Classes.Menu.Styles;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Menu;
/// <summary>
/// A menu that displays the options in a grid with the specified width.
/// </summary>
internal class GridMenu : IMenu
{
    public bool Opened { get; set; }
    public List<Option> Options { get; }
    public string? Message { get; set; }
    public ICloseAction CloseAction { get; set; } = new StandardClose();
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;
    public IOptionStyle OptionStyle { get; set; } = new DefaultStyle("  ", "> ", "  ", " <");

    /// <summary>
    /// The amount of columns that the menu should have.
    /// </summary>
    public int Width { get; set; } = 15;

    private int _selected;
    private int Selected
    {
        get => _selected;
        set
        {
            if (value < 0)
            {
                _selected = 0;
            }
            else if (value >= Options.Count)
            {
                _selected = Options.Count - 1;
            }
            else
            {
                _selected = value;
            }
        }
    }

    public GridMenu(List<Option> options)
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

        List<MultilineString> columns = new();

        for (int i = 0; i < Options.Count; i++)
        {
            MultilineString option = new(OptionStyle.Style(Options[i], i == Selected));

            if (i < Width)
            {
                columns.Add(option);
            }
            else
            {
                columns[i % Width] = columns[i % Width].CombineVertically(option, HorizontalAlignment.Center, 1);
            }
        }

        Console.WriteLine(MultilineString.Join(columns, VerticalAlignment.Top, " "));
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
            else if (keyInfo.Key == ConsoleKey.LeftArrow
                || keyInfo.Key == ConsoleKey.A)
            {
                Selected--;
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow
                || keyInfo.Key == ConsoleKey.D)
            {
                Selected++;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow
                || keyInfo.Key == ConsoleKey.W)
            {
                Selected -= Width;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow
                || keyInfo.Key == ConsoleKey.S)
            {
                Selected += Width;
            }
        }
    }
}
