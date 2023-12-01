using MatchstickPuzzle.Classes;
using MatchstickPuzzle.Classes.Game;
using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Menu;
using MatchstickPuzzle.Classes.Menu.CloseActions;

Console.Title = "Mathsticks";
Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Puzzle> puzzles = new()
{
    // 6 + 4 = 4
    new Puzzle(new Equation(new Addition(new Number(6), new Number(4)), new Number(4)), 1),
    // 3 + 1 = 3
    new Puzzle(new Equation(new Addition(new Number(3), new Number(1)), new Number(3)), 1),
    // 6 - 6 = 3
    new Puzzle(new Equation(new Subtraction(new Number(6), new Number(6)), new Number(3)), 1),
    // 9 - 5 = 8
    new Puzzle(new Equation(new Subtraction(new Number(9), new Number(5)), new Number(8)), 1),
    // 13 + 4 = 12
    new Puzzle(new Equation(new Addition(new Number(13), new Number(4)), new Number(12)), 2),
    // 5 * 2 = 73
    new Puzzle(new Equation(new Multiplication(new Number(5), new Number(2)), new Number(73)), 2),
    // 6 + 3 = 1 + 1
    new Puzzle(new Equation(new Addition(new Number(6), new Number(3)), new Addition(new Number(1), new Number(1))), 1),
    // 4 * 7 = 2 + 3
    new Puzzle(new Equation(new Multiplication(new Number(4), new Number(7)), new Addition(new Number(2), new Number(3))), 2),
    // 5 - 3 = 2 * 9
    new Puzzle(new Equation(new Subtraction(new Number(5), new Number(3)), new Multiplication(new Number(2), new Number(9))), 1),
    // 78 / 3 = 5
    new Puzzle(new Equation(new Division(new Number(78), new Number(3)), new Number(5)), 1),
    // 1 / 1 = 4
    new Puzzle(new Equation(new Division(new Number(1), new Number(1)), new Number(4)), 2),
    // 4 - 1 = 7 * 8
    new Puzzle(new Equation(new Subtraction(new Number(4), new Number(1)), new Multiplication(new Number(7), new Number(8))), 2),
    // 6 / 8 = 9
    new Puzzle(new Equation(new Division(new Number(6), new Number(8)), new Number(9)), 2),
    // 5 + 2 + 7 = 9
    new Puzzle(new Equation(new Addition(new Addition(new Number(5), new Number(2)), new Number(7)), new Number(9)), 1),
    // 8 * 3 + 8 = 5
    new Puzzle(new Equation(new Addition(new Multiplication(new Number(8), new Number(3)), new Number(8)), new Number(5)), 2),
};

GameData gameData = new();
AppDataHandler appDataHandler = new("Mathsticks");
LoadGameData();

// Start menu
List<MenuOption> startOptions = new()
{
    new("Play", OpenPuzzleMenu),
    new("Help", ViewHelp),
    new("Reset", ResetData)
};

ListMenu startMenu = new(startOptions)
{
    Message = "MATHSTICKS",
    CloseAction = new ConfirmClose(SaveGameData),
    CloseWithEscape = true,
    CloseAfterAction = false,
};

startMenu.Options.Add(new("Quit", () => startMenu.CloseAction.Close(startMenu)));
startMenu.Open();

void OpenPuzzleMenu()
{
    GridMenu puzzleMenu = new(new())
    {
        Message = "Choose a puzzle!",
        CloseWithEscape = true,
        CloseAfterAction = false,
        Width = 10
    };

    foreach (Puzzle puzzle in puzzles)
    {
        int puzzleIndex = puzzles.IndexOf(puzzle);
        string checkMark = puzzle.Solved ? "#" : "";
        bool unlocked = puzzles.Take(puzzleIndex).Count((puzzle) => puzzle.Solved) > puzzleIndex - 3;

        string finalText = unlocked ? $"{puzzleIndex + 1}{checkMark}" : "╳";

        puzzleMenu.Options.Add(new(finalText, () =>
        {
            if (unlocked)
            {
                puzzleMenu.Opened = false;
                PlayPuzzle(puzzle);
            }
        }));
    }

    SaveGameData();
    puzzleMenu.Open();
}

void PlayPuzzle(Puzzle puzzle)
{
    puzzle.Start();

    if (!puzzle.Equal)
    {
        OpenPuzzleMenu();
        return;
    }

    if (!puzzle.Solved)
    {
        if (ListMenu.YesOrNo("Do you want to retry the same level?"))
        {
            PlayPuzzle(puzzle);
        }
        else
        {
            OpenPuzzleMenu();
        }

        return;
    }

    Puzzle? nextUnsolved = puzzles.Skip(puzzles.IndexOf(puzzle)).ToList().Find((puzzle) => !puzzle.Solved)
        ?? puzzles.Find((puzzle) => !puzzle.Solved);

    if (nextUnsolved == null)
    {
        OpenPuzzleMenu();
        return;
    }

    if (ListMenu.YesOrNo($"Do you want to go to level {puzzles.IndexOf(nextUnsolved) + 1}"))
    {
        PlayPuzzle(nextUnsolved);
    }
    else
    {
        OpenPuzzleMenu();
    }
}

void ViewHelp()
{
    Console.Clear();
    Console.WriteLine(
        "Rules:\n" +
        "The game has many levels, each with an unique puzzle. For each puzzle you have a certain number of\n" +
        "moves, shown at the top of the screen, to move the matchsticks around so that the equation shown is\n" +
        "equal. You can play 3 levels more than those you have completed, meaning that if you are stuck\n" +
        "you can try some other uncompleted stages.\n" +
        "Good luck!\n" +
        "\n" +
        "Controls:\n" +
        "WASD or ↑←↓→: Choose stick and navigate menus.\n" +
        "SPACE or ENTER: Select stick or menu option.\n" +
        "ESCAPE: Exit/Go back.\n" +
        "CTRL+Z: Undo move.\n");
    ConsoleExtension.WriteColoredLine("Press any key to go back!", ConsoleColor.Cyan);
    Console.ReadKey(true);
}

void ResetData()
{
    if (!ListMenu.YesOrNo("Are you sure you want to reset all progress?", true)
        || !ListMenu.YesOrNo("Current progress can not be retrieved, are you sure?", true))
    {
        return;
    }

    foreach (Puzzle puzzle in puzzles)
    {
        puzzle.Solved = false;
    }

    SaveGameData();
}

void SaveGameData()
{
    gameData.CompletedLevels = puzzles
        .FindAll((puzzle) => puzzle.Solved)
        .Select((puzzle) => puzzles.IndexOf(puzzle))
        .ToList();

    appDataHandler.WriteToXmlFile("gameData.xml", gameData);
}

void LoadGameData()
{
    try
    {
        GameData? loadedGameData = appDataHandler.ReadFromXmlFile<GameData>("gameData.xml");

        if (loadedGameData != null)
        {
            gameData = loadedGameData;
        }
    }
    catch (Exception) { }

    foreach (int completedLevel in gameData.CompletedLevels)
    {
        puzzles[completedLevel].Solved = true;
    }
}
