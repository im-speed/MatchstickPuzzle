using MatchstickPuzzle.Classes.Game;
using MatchstickPuzzle.Classes.Game.Expressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Puzzle> puzzles = new()
{
    new(new(new Addition(new Number(6), new Number(4)), new Number(4)), 1),
    new(new(new Subtraction(new Number(0), new Number(6)), new Number(3)), 1)
};

foreach (Puzzle puzzle in puzzles)
{
    puzzle.Start();

    Console.Clear();
}
