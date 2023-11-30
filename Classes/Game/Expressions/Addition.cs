using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game.Expressions;

/* 1: Arv av klasser
 * 2: Alla uttryck ärver av klassen Expression
 * 3: Det gör att uttrycken bara behöver implementera kod för att beräkna sitt Value och returnera
 *    symbolen som skrivs ut mellan vänstra och högra värdena.
 */
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
