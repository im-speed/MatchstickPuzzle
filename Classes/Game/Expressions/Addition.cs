using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Expressions;

internal class Addition : Expression
{
    public override double? Value
        => LeftValue.Value + RightValue.Value;

    protected override MultilineString Symbol
        => new(
            "  |  \n" +
            "──┼──\n" +
            "  |  ");

    public Addition(IValue leftValue, IValue rightValue)
        : base(leftValue, rightValue) { }
}
