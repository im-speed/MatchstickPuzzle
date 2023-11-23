namespace MatchstickPuzzle.Classes.Menu;

internal class ListMenu
{
    private bool opened;
    public List<MenuOption> Options { get; }
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;
    public string OptionPrefix { get; set; } = " ";
    public string SelectedPrefix { get; set; } = ">";

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

    public ListMenu(List<MenuOption> options)
    {
        Options = options;
    }

    public void Open()
    {
        if (Options.Count < 1)
        {
            throw new NullReferenceException("Can not opan a menu with no options.");
        }

        opened = true;
        while (opened)
        {
            Console.Clear();

            for (int i = 0; i < Options.Count; i++)
            {
                if (i == Selected)
                {
                    Console.WriteLine($"{SelectedPrefix} {Options[i].Text}");
                }
                else
                {
                    Console.WriteLine($"{OptionPrefix} {Options[i].Text}");
                }
            }

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    if (CloseWithEscape)
                        Close();
                    break;

                case ConsoleKey.DownArrow:
                    Selected++;
                    break;

                case ConsoleKey.UpArrow:
                    Selected--;
                    break;

                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    Options[Selected].Action();
                    if (CloseAfterAction)
                        Close();
                    break;
            }
        }
    }

    public void Close()
    {
        opened = false;
    }
}
