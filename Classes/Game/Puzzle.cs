using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.Game.Sticks;

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

    public Puzzle(Equation equation, int moves)
    {
        this.moves = moves;
        this.equation = equation;
        _selectedStick = equation.Sticks[0][0];
    }

    public void Start()
    {
        while (!equation.Equal)
        {
            SelectedStick = equation.Sticks[stickY][stickX];

            Console.Clear();
            Console.WriteLine(equation.Shape);

            switch (Console.ReadKey().Key)
            {
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
    }

    private void MoveLeft()
    {
        List<IStick> row = equation.Sticks[stickY];

        for (int i = stickX - 1; i >= 0; i--)
        {
            if (row[i].Empty == holdingStick)
            {
                stickX = i;
                break;
            }
        }
    }

    private void MoveRight()
    {
        List<IStick> row = equation.Sticks[stickY];

        for (int i = stickX + 1; i < row.Count; i++)
        {
            if (row[i].Empty == holdingStick)
            {
                stickX = i;
                break;
            }
        }
    }

    private void MoveUp()
    {
        for (int i = stickY - 1; i >= 0; i--)
        {
            double widthScale = (double)equation.Sticks[i].Count / equation.Sticks[stickY].Count;
            int column = (int)(stickX * widthScale);

            if (equation.Sticks[i][column].Empty == holdingStick)
            {
                stickY = i;
                stickX = column;
                break;
            }
        }
    }

    private void MoveDown()
    {
        for (int i = stickY + 1; i < equation.Sticks.Count; i++)
        {
            double widthScale = (double)equation.Sticks[i].Count / equation.Sticks[stickY].Count;
            int column = (int)(stickX * widthScale);

            if (equation.Sticks[i][column].Empty == holdingStick)
            {
                stickY = i;
                stickX = column;
                break;
            }
        }
    }
}
