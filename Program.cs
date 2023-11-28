using MatchstickPuzzle.Classes;
using MatchstickPuzzle.Classes.Game;
using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Menu;
using MatchstickPuzzle.Classes.Menu.CloseActions;

Console.Title = "Mathsticks";
Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Puzzle> puzzles = new()
{
    new Puzzle(new Equation(new Addition(new Number(6), new Number(4)), new Number(4)), 1),
    new Puzzle(new Equation(new Subtraction(new Number(0), new Number(6)), new Number(3)), 1),
    new Puzzle(new Equation(new Addition(new Number(13), new Number(4)), new Number(12)), 2),
};

GameData gameData = new();
AppDataHandler appDataHandler = new("Mathsticks");
LoadGameData();

// Start menu
List<MenuOption> startOptions = new()
{
    new("Play", OpenPuzzleMenu),
    new("Help", ViewHelp)
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
    List<MenuOption> puzzleMenuOptions = new();

    foreach (Puzzle puzzle in puzzles)
    {
        int puzzleIndex = puzzles.IndexOf(puzzle);
        string checkMark = puzzle.Solved ? "#" : "";
        bool unlocked = puzzles.Take(puzzleIndex).Count((puzzle) => puzzle.Solved) > puzzleIndex - 3;

        string finalText = unlocked ? $"{puzzleIndex + 1}{checkMark}" : "╳";

        puzzleMenuOptions.Add(new(finalText, () => PlayPuzzle(puzzle)));
    }

    GridMenu puzzleMenu = new(puzzleMenuOptions)
    {
        Message = "Choose a puzzle!",
        CloseWithEscape = true
    };

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
        if (ListMenu.YesOrNo("Do you want to restart the same level?"))
        {
            PlayPuzzle(puzzle);
        }

        OpenPuzzleMenu();
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

    OpenPuzzleMenu();
}

void ViewHelp()
{
    Console.Clear();
    Console.WriteLine(
        "" +
        "Controls:\n" +
        "WASD or ↑←↓→: Choose stick and navigate menus.\n" +
        "SPACE or ENTER: Select stick or menu option." +
        "ESCAPE: Exit/Go back." +
        "CTRL+Z: Undo move.\n");
    ConsoleExtension.WriteColoredLine("Press any key to go back!", ConsoleColor.Cyan);
    Console.ReadKey(true);
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
