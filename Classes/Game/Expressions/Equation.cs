using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Expressions;

internal class Equation : Expression
{
    protected override MultilineString Symbol
        => new(
            "─────\n" +
            "─────");

    public override double? Value
        => LeftValue.Value - RightValue.Value;

    public bool Equal
        => Value == 0;

    public Equation(IValue leftValue, IValue rightValue)
        : base(leftValue, rightValue) { }
}
