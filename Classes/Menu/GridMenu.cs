using MatchstickPuzzle.Classes.Menu.CloseActions;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Menu;
internal class GridMenu : IMenu
{
    public bool Opened { get; set; }
    public List<MenuOption> Options { get; }
    public string? Message { get; set; }
    public ICloseAction CloseAction { get; set; } = new StandardClose();
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;
    public string OptionPrefix { get; set; } = " ";
    public string SelectedPrefix { get; set; } = ">";
    public string OptionSuffix { get; set; } = " ";
    public string SelectedSuffix { get; set; } = "<";
    public int Width { get; set; } = 15;

    private int _selected;
    public int Selected
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

    public GridMenu(List<MenuOption> options)
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
            MultilineString option = new(
                $"{(i == Selected ? SelectedPrefix : OptionPrefix)}" +
                $" {Options[i].Text}" +
                $" {(i == Selected ? SelectedSuffix : OptionSuffix)}");

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

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    if (CloseWithEscape)
                        CloseAction.Close(this);
                    break;

                case ConsoleKey.LeftArrow:
                    Selected--;
                    break;

                case ConsoleKey.RightArrow:
                    Selected++;
                    break;

                case ConsoleKey.UpArrow:
                    Selected -= 15;
                    break;

                case ConsoleKey.DownArrow:
                    Selected += 15;
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    Options[Selected].Action();
                    if (CloseAfterAction)
                        CloseAction.Close(this);
                    break;
            }
        }
    }
}
