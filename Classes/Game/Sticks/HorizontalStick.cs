using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Sticks;

internal class HorizontalStick : IStick
{
    public bool Selected { get; set; }

    public bool Empty { get; set; }

    public MultilineString Shape
    {
        get
        {
            if (Empty)
            {
                return Selected ? new("→     ←") : new("       ");
            }

            return Selected ? new("═══════") : new("───────");
        }
    }
}
