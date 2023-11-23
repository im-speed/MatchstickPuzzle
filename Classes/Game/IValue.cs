using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal interface IValue
{
    double? Value { get; }
    MultilineString Shape { get; }
    List<List<IStick>> Sticks { get; }
}