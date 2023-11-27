﻿using MatchstickPuzzle.Classes.Menu.CloseActions;

namespace MatchstickPuzzle.Classes.Menu;

internal class ListMenu : IMenu
{
    public bool Opened { get; set; }
    public List<MenuOption> Options { get; }
    public string? Message { get; set; }
    public ICloseAction CloseAction { get; set; } = new StandardClose();
    public bool CloseWithEscape { get; set; } = false;
    public bool CloseAfterAction { get; set; } = true;
    public string OptionPrefix { get; set; } = " ";
    public string SelectedPrefix { get; set; } = ">";
    public string OptionSuffix { get; set; } = "";
    public string SelectedSuffix { get; set; } = "";

    private int _selected;
    public int Selected
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

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    if (CloseWithEscape)
                        CloseAction.Close(this);
                    break;

                case ConsoleKey.UpArrow:
                    Selected--;
                    break;

                case ConsoleKey.DownArrow:
                    Selected++;
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
