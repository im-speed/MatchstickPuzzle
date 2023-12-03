using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.Menu;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal class Puzzle
{
    private readonly List<List<bool>> originalLayout = new();
    private readonly Equation equation;
    private readonly int maxMoves;
    private readonly List<Tuple<int, int>> moveHistory = new();
    private bool holdingStick;
    private int stickX = 0;
    private int stickY = 0;

    private List<List<IStick>> Sticks
        => equation.Sticks;

    private int Moves
        => moveHistory.Count / 2;

    private IStick _selectedStick;
    private IStick SelectedStick
    {
        get => _selectedStick;
        set
        {
            _selectedStick.Selected = false;
            _selectedStick = value;
            _selectedStick.Selected = true;
        }
    }

    /// <summary>
    /// True if the puzzle has been solved in the required amount of moves.
    /// </summary>
    public bool Solved { get; set; }

    /// <summary>
    /// True if the equation has been solved, even if done in more moves than allowed.
    /// </summary>
    public bool Equal
        => equation.Equal;

    /// <inheritdoc cref="Expression.Shape"/>
    public MultilineString Shape
        => equation.Shape;

    /// <summary>
    /// Creates a puzzle that once started will let the player move around sticks to try and make the equation equal.
    /// </summary>
    /// <param name="equation">The equation to solve.</param>
    /// <param name="maxMoves">Maximum moves to solve the puzzle in.</param>
    public Puzzle(Equation equation, int maxMoves)
    {
        this.maxMoves = maxMoves;
        this.equation = equation;
        _selectedStick = Sticks[0][0];

        foreach (List<IStick> stickRow in Sticks)
        {
            List<bool> row = new();

            foreach (IStick stick in stickRow)
            {
                row.Add(stick.Empty);
            }

            originalLayout.Add(row);
        }
    }

    /// <summary>
    /// Starts the puzzle and will run a loop until the puzzle is solved or the user exits.
    /// </summary>
    public void Start()
    {
        // Reset the puzzle
        moveHistory.Clear();
        holdingStick = false;
        ResetSticks();
        ChooseFirstStick();

        while (!Equal || holdingStick)
        {
            SelectedStick = Sticks[stickY][stickX];

            Console.Clear();
            Console.WriteLine($"Moves left: {maxMoves - moveHistory.Count / 2}");
            ConsoleExtension.WriteColoredLine(
                !holdingStick
                ? "Select a stick to pick up!"
                : "Place stick!",
                ConsoleColor.Cyan);
            Console.WriteLine(equation.Shape);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                if (ListMenu.YesOrNo("Are you sure you wish to exit?"))
                    return;
            }
            else if (keyInfo.Key == ConsoleKey.Enter
                || keyInfo.Key == ConsoleKey.Spacebar)
            {
                ChooseStick();
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow
                || keyInfo.Key == ConsoleKey.A)
            {
                MoveLeft();
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow
                || keyInfo.Key == ConsoleKey.D)
            {
                MoveRight();
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow
                || keyInfo.Key == ConsoleKey.W)
            {
                MoveUp();
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow
                || keyInfo.Key == ConsoleKey.S)
            {
                MoveDown();
            }
            else if (keyInfo.Key == ConsoleKey.Z
                && keyInfo.Modifiers == ConsoleModifiers.Control)
            {
                UndoMoves(holdingStick ? 1 : 2);
            }
        }

        if (Moves <= maxMoves)
        {
            Solved = true;
        }

        FinishGame();
    }

    private void FinishGame()
    {
        Console.Clear();

        if (Moves > maxMoves)
        {
            Console.WriteLine(
                $"Nice try!\n" +
                $"But if you wish to beat the level you have to solve the puzzle in the specified amount of moves.\n\n" +
                $"{Shape}\n");

            ConsoleExtension.WriteColoredLine("Press any key to continue!", ConsoleColor.Cyan);
            Console.ReadKey(true);
            return;
        }

        ConsoleExtension.WriteColoredLine("Congratulations on beating the puzzle!\n", ConsoleColor.Green);
        Console.WriteLine($"{Shape}\n");

        ConsoleExtension.WriteColoredLine("Press any key to continue!", ConsoleColor.Cyan);
        Console.ReadKey(true);
    }

    private void ResetSticks()
    {
        for (int i = 0; i < originalLayout.Count; i++)
        {
            for (int j = 0; j < originalLayout[i].Count; j++)
            {
                Sticks[i][j].Empty = originalLayout[i][j];
            }
        }
    }

    private void ChooseFirstStick()
    {
        List<Tuple<int, int>> selectionPriorities = new()
        {
            new(0, 0),
            new(0, 1),
            new(1, 1)
        };

        foreach (Tuple<int, int> selection in selectionPriorities)
        {
            (stickX, stickY) = selection;

            if (!Sticks[stickY][stickX].Empty)
            {
                return;
            }
        }
    }

    private void ChooseStick()
    {
        moveHistory.Add(new(stickX, stickY));
        holdingStick = !holdingStick;
        SelectedStick.Empty = !SelectedStick.Empty;
    }

    private void UndoMoves(int moves)
    {
        if (moveHistory.Count == 0 || moves == 0)
        {
            return;
        }

        (stickX, stickY) = moveHistory.Last();
        SelectedStick = Sticks[stickY][stickX];

        holdingStick = !holdingStick;
        SelectedStick.Empty = !SelectedStick.Empty;

        moveHistory.RemoveAt(moveHistory.Count - 1);

        UndoMoves(moves - 1);
    }

    private void MoveLeft()
    {
        List<IStick> row = Sticks[stickY];

        for (int x = stickX - 1; x >= 0; x--)
        {
            if (row[x].Empty == holdingStick)
            {
                stickX = x;
                return;
            }
        }
    }

    private void MoveRight()
    {
        List<IStick> row = Sticks[stickY];

        for (int x = stickX + 1; x < row.Count; x++)
        {
            if (row[x].Empty == holdingStick)
            {
                stickX = x;
                return;
            }
        }
    }

    private void MoveUp()
    {
        MoveVertically(stickY - 1, (y) => y >= 0, -1);
    }

    private void MoveDown()
    {
        MoveVertically(stickY + 1, (y) => y < Sticks.Count, 1);
    }

    private void MoveVertically(int start, Func<int, bool> endCondition, int step)
    {
        int xScopeIncrease = 0;

        while (xScopeIncrease < Sticks.Max((row) => row.Count))
        {
            for (int y = start; endCondition(y); y += step)
            {
                for (int isNegative = -1; isNegative <= 1; isNegative += 2)
                {
                    double widthScale = (double)Sticks[y].Count / Sticks[stickY].Count;
                    int x = (int)(stickX * widthScale) + isNegative * xScopeIncrease;

                    if (x < 0 || Sticks[y].Count <= x)
                    {
                        continue;
                    }

                    if (Sticks[y][x].Empty == holdingStick)
                    {
                        stickY = y;
                        stickX = x;
                        return;
                    }

                    // Forces the selection to try both vertical sticks below/above a horizontal stick before continuing.
                    if (y == start && widthScale > 1 && Sticks[y][x + 1].Empty == holdingStick)
                    {
                        stickY = y;
                        stickX = x + 1;
                        return;
                    }
                }
            }

            xScopeIncrease++;
        }
    }
}
