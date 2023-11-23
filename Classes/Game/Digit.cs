using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal class Digit
{
    private readonly List<IStick> sticks;
    private static readonly List<List<bool>> shapeLookup = new()
    {
        // Index for each segment
        //   -- 0 --
        //  |       |
        //  1       2
        //  |       |
        //   -- 3 --
        //  |       |
        //  4       5
        //  |       |
        //   -- 6 --
        new() {true,  true,  true,  false,  true,  true,  true},
        new() {false,  false,  true,  false,  false,  true,  false},
        new() {true,  false,  true,  true,  true,  false,  true},
        new() {true,  false,  true,  true,  false,  true,  true},
        new() {false,  true,  true,  true,  false,  true,  false},
        new() {true,  true,  false,  true,  false,  true,  true},
        new() {true,  true,  false,  true,  true,  true,  true},
        new() {true,  false,  true,  false,  false,  true,  false},
        new() {true,  true,  true,  true,  true,  true,  true},
        new() {true,  true,  true,  true,  false,  true,  true},
    };

    public int? Value
    {
        get
        {
            List<bool> currentShape = sticks.Select((stick) => !stick.Empty).ToList();

            for (int i = 0; i < shapeLookup.Count; i++)
            {
                bool matches = true;
                List<bool> shape = shapeLookup[i];

                for (int j = 0; j < shape.Count; j++)
                {
                    matches = matches && shape[j] == currentShape[j];
                }

                if (matches)
                {
                    return i;
                }
            }

            return null;
        }
    }

    public MultilineString Shape
    {
        get
        {
            List<MultilineString> shapes = sticks.Select((shape) => shape.Shape).ToList();

            string verticalSticksSeparator = "".PadLeft(new HorizontalStick().Shape.Width);

            return shapes[0]
                .AlignHorizontally(HorizontalAlignment.Center, shapes[0].Width + 2)
                .CombineVertically(shapes[1].CombineHorizontally(shapes[2], verticalSticksSeparator))
                .CombineVertically(shapes[3], HorizontalAlignment.Center)
                .CombineVertically(shapes[4].CombineHorizontally(shapes[5], verticalSticksSeparator))
                .CombineVertically(shapes[6], HorizontalAlignment.Center);
        }
    }

    public Digit(char digit)
    {
        List<bool> shape;

        try
        {
            shape = shapeLookup[int.Parse(digit.ToString())];
        }
        catch (Exception)
        {
            throw new ArgumentException("Argument digit was not a digit.");
        }

        sticks = new();
        for (int i = 0; i <= 6; i++)
        {
            sticks.Add(i % 3 == 0 ? new HorizontalStick() : new VerticalStick());
            sticks[i].Empty = !shape[i];
        }
    }
}
