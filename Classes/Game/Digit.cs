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

    /* 1: Computed properties
     * 2: Value är en beräknad egenskap som istället för att hålla ett värde kollar om den nuvarande
     *    formen på siffran matchar ett av dem i lookup ovan.
     * 3: Anledningen till att göra så är att värdet på Digit är inte lagrat som ett heltal, utan 
     *    påverkas av vad användaren flyttar för tändstickor.
     */

    /// <summary>
    /// Returns the current value (0-9) of the digit, -1 if empty, or null if it is an invalid shape.
    /// </summary>
    public int? Value
    {
        get
        {
            List<bool> currentShape = sticks.Select((stick) => !stick.Empty).ToList();

            if (currentShape.All((stick) => !stick))
            {
                return -1;
            }

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

    /// <summary>
    /// All seven sticks of the digit split into 5 rows.
    /// </summary>
    public List<List<IStick>> Sticks
        => new()
        {
            new() { sticks[0] },
            new() { sticks[1], sticks[2] },
            new() { sticks[3] },
            new() { sticks[4], sticks[5] },
            new() { sticks[6] }
        };

    /// <summary>
    /// Returns the multiline string representation of the object.
    /// </summary>
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
