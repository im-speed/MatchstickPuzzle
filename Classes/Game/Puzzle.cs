using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.Menu;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal class Puzzle
{
    private readonly List<List<bool>> originalLayout = new();
    private readonly Equation equation;
    private readonly int moves;
    private readonly List<Tuple<int, int>> moveHistory = new();
    private bool holdingStick;
    private int stickX = 0;
    private int stickY = 0;

    private List<List<IStick>> Sticks
        => equation.Sticks;

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

    public bool Solved { get; set; }

    public bool Equal
        => equation.Equal;

    public MultilineString Shape
        => equation.Shape;

    public Puzzle(Equation equation, int moves)
        : this(equation, moves, false)
    {
    }

    public Puzzle(Equation equation, int moves, bool solved)
    {
        this.moves = moves;
        this.equation = equation;
        Solved = solved;
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

    public void Start()
    {
        // Reset the puzzle
        moveHistory.Clear();
        (stickX, stickY) = (0, 0);
        holdingStick = false;
        ResetSticks();

        while (!Equal || holdingStick)
        {
            SelectedStick = Sticks[stickY][stickX];

            Console.Clear();
            Console.WriteLine($"Moves left: {moves - moveHistory.Count / 2}");
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
                UndoMove();
            }
        }

        if (moveHistory.Count >= 0)
        {
            Solved = true;
        }
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

    private void ChooseStick()
    {
        moveHistory.Add(new(stickX, stickY));
        holdingStick = !holdingStick;
        SelectedStick.Empty = !SelectedStick.Empty;
    }

    private void UndoMove()
    {
        UndoMoves(holdingStick ? 1 : 2);
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
