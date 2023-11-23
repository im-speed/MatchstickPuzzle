using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.Menu;

namespace MatchstickPuzzle.Classes.Game;

internal class Puzzle
{
    private int moves;
    private readonly Equation equation;
    private bool holdingStick;
    private int stickX = 0;
    private int stickY = 0;

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

    public Puzzle(Equation equation, int moves)
        : this(equation, moves, false)
    {
    }

    public Puzzle(Equation equation, int moves, bool solved)
    {
        this.moves = moves;
        this.equation = equation;
        Solved = solved;
        _selectedStick = equation.Sticks[0][0];
    }

    public void Start()
    {
        while (!equation.Equal || holdingStick)
        {
            SelectedStick = equation.Sticks[stickY][stickX];

            Console.Clear();
            ConsoleExtension.WriteColoredLine(
                !holdingStick
                ? "Select a stick to pick up!"
                : "Place stick!",
                ConsoleColor.Cyan);
            Console.WriteLine(equation.Shape);

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Escape:
                    if (ConfirmExit())
                        return;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    holdingStick = !holdingStick;
                    SelectedStick.Empty = !SelectedStick.Empty;
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight();
                    break;
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
            }
        }

        Solved = true;
    }

    private bool ConfirmExit()
    {
        bool confirmed = false;
        List<MenuOption> options = new()
        {
            new("Yes", () => confirmed = true),
            new("No", () => confirmed = false)
        };

        ListMenu yesOrNo = new(options)
        {
            CloseWithEscape = true
        };
        yesOrNo.Open();

        return confirmed;
    }

    private void MoveLeft()
    {
        List<IStick> row = equation.Sticks[stickY];

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
        List<IStick> row = equation.Sticks[stickY];

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
        MoveVertically(stickY + 1, (y) => y < equation.Sticks.Count, 1);
    }

    private void MoveVertically(int start, Func<int, bool> endCondition, int step)
    {
        int xScopeIncrease = 0;

        while (xScopeIncrease < equation.Sticks.Max((row) => row.Count))
        {
            for (int y = start; endCondition(y); y += step)
            {
                for (int isNegative = -1; isNegative <= 1; isNegative += 2)
                {
                    double widthScale = (double)equation.Sticks[y].Count / equation.Sticks[stickY].Count;
                    int x = (int)(stickX * widthScale) + isNegative * xScopeIncrease;

                    if (x < 0 || equation.Sticks[y].Count <= x)
                    {
                        continue;
                    }

                    if (equation.Sticks[y][x].Empty == holdingStick)
                    {
                        stickY = y;
                        stickX = x;
                        return;
                    }
                }
            }

            xScopeIncrease++;
        }
    }
}
