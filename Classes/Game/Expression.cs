using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

/* 1: Abstrakta klasser
 * 2: Expression ärvs av alla uttryck och Equation och implementerar själv en konstruktor som
 *    sätter vänster och höger värde, en egenskap som returnerar hela uttryckets sträng och
 *    en egenskap som returnerar vänster och höger värdes tändstickor.
 * 3: Anledningen till att använda en abstrakt klass är att alla uttryck delar den mesta kod.
 *    Undantagen är Symbol, vilket är en sträng som representerar +, -, * eller /, och Value
 *    som bara beräknar vad uttrycket ska göra.
 */
internal abstract class Expression : IValue
{
    /* 1: Åtkomstmodifieraren `protected` 
     * 2: Symbol är protected och används i Expression för att skapa den fulla Shape, och den
     *    har ett värde som är satt i Expression:s sub-typer.
     * 3: Eftersom Symbol inte bör behövas andvändas utanför den fulla Shape så finns det ingen
     *    anledning att göra den public, men den behöver fortfarande kommas åt av sub-typerna så
     *    att den kan anges ett värde.
     */

    /// <summary>
    /// The shape to draw between the left- and right-hand sides.
    /// </summary>
    protected abstract MultilineString Symbol { get; }

    /* 1: Subtypspolimorfism 
     * 2: LeftValue och RightValue används av Expressions sub-typer för att beräkna uttryckens
     *    värde.
     * 3: Dessa använder sig av subtypspolimorfism eftersom uttrycken bara behöver värdet på
     *    vänster och höger sida, inte hur det beräknades.
     */

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
