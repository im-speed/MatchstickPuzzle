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

    /* 1: Konstruktor-kedjning
     * 2: I varje uttryck finns det en konstruktor som anropar Expression:s konstruktor.
     * 3: För tillfället gör detta väldigt lite, eftersom allt som händer i Expression:s 
     *    konstruktor är att vänster och höger värde tilldelas. Men det sparar ändå lite
     *    kod, och om jag vill lägga till något mer i Expression så kommer det då även
     *    vara med i alla sub-klasser.
     */

    public Addition(IValue leftValue, IValue rightValue)
        : base(leftValue, rightValue) { }
}
