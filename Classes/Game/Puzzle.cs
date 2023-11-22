using MatchstickPuzzle.Classes.Game.Expressions;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game
{
    internal class Puzzle
    {
        private int moves;
        private readonly Equation equation;

        public bool Solved
            => equation.Equal;

        public MultilineString Shape
            => equation.Shape;

        public Puzzle(Equation equation, int moves)
        {
            this.moves = moves;
            this.equation = equation;
        }
    }
}
