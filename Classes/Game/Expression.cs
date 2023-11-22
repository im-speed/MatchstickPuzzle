using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game
{
    internal abstract class Expression : IValue
    {
        protected abstract MultilineString Symbol { get; }
        protected IValue LeftValue { get; init; }
        protected IValue RightValue { get; init; }
        public abstract double? Value { get; }

        public MultilineString Shape
            => LeftValue.Shape
            .CombineHorizontally(Symbol, VerticalAlignment.Middle, "   ")
            .CombineHorizontally(RightValue.Shape, "   ");

        public Expression(IValue leftValue, IValue rightValue)
        {
            LeftValue = leftValue;
            RightValue = rightValue;
        }
    }
}
