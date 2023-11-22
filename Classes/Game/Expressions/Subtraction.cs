using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Expressions
{
    internal class Subtraction : Expression
    {
        public override double? Value
            => LeftValue.Value - RightValue.Value;

        protected override MultilineString Symbol
            => new("─────");

        public Subtraction(IValue leftValue, IValue rightValue)
            : base(leftValue, rightValue) { }
    }
}
