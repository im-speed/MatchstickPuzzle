using MatchstickPuzzle.Classes.Game;
using MatchstickPuzzle.Classes.Game.Expressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Puzzle> puzzles = new()
{
    new(new(new Addition(new Number(6), new Number(4)), new Number(4)), 1)
};

Console.WriteLine(puzzles[0].Shape);
