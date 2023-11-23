using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Sticks;

internal class VerticalStick : IStick
{
    public bool Selected { get; set; }

    public bool Empty { get; set; }

    public MultilineString Shape
    {
        get
        {
            if (Empty)
            {
                return Selected ? new("↓\n \n↑") : new(" \n \n ");
            }

            return Selected ? new("║\n║\n║") : new("|\n|\n|");
        }
    }
}
