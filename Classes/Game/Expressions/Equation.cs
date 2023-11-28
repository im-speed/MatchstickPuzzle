using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Expressions;

internal class Equation : Expression
{
    protected override MultilineString Symbol
        => new(
            "─────\n" +
            "─────");

    /// <summary>
    /// Returns the difference between the two sides.
    /// </summary>
    public override double? Value
        => LeftValue.Value - RightValue.Value;

    /// <summary>
    /// Returns true if both sides of the equation are equal.
    /// </summary>
    public bool Equal
        => Value == 0;

    public Equation(IValue leftValue, IValue rightValue)
        : base(leftValue, rightValue) { }
}
