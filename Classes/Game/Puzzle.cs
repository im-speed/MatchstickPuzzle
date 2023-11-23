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
                    stickX--;
                    break;
                case ConsoleKey.UpArrow:
                    stickY--;
                    break;
                case ConsoleKey.RightArrow:
                    stickX++;
                    break;
                case ConsoleKey.DownArrow:
                    stickY++;
                    break;
            }
        }
    }
}
