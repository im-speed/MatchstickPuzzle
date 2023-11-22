using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Sticks
{
    internal interface IStick
    {
        bool Selected { get; set; }
        bool Empty { get; set; }
        MultilineString Shape { get; }
    }
}
