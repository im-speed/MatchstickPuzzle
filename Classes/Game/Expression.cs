using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal abstract class Expression : IValue
{
    /// <summary>
    /// The shape to draw between the left- and right-hand sides.
    /// </summary>
    protected abstract MultilineString Symbol { get; }

    /// <summary>
    /// The left-hand side of the expression.
    /// </summary>
    protected IValue LeftValue { get; init; }

    /// <summary>
    /// The right-hand side of the expression.
    /// </summary>
    protected IValue RightValue { get; init; }

    public abstract double? Value { get; }

    public MultilineString Shape
        => LeftValue.Shape
        .CombineHorizontally(Symbol, VerticalAlignment.Middle, "   ")
        .CombineHorizontally(RightValue.Shape, "   ");

    public List<List<IStick>> Sticks
    {
        get
        {
            List<List<IStick>> sticks = LeftValue.Sticks;

            for (int i = 0; i < LeftValue.Sticks.Count; i++)
            {
                sticks[i].AddRange(RightValue.Sticks[i]);
            }

            return sticks;
        }
    }

    public Expression(IValue leftValue, IValue rightValue)
    {
        LeftValue = leftValue;
        RightValue = rightValue;
    }
}
