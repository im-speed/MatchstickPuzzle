using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game
{
    internal interface IValue
    {
        double? Value { get; }
        MultilineString Shape { get; }
    }
}