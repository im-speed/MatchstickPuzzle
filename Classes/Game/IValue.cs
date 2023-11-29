using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

/* 1: Interfaces
 * 2: IValue är ett gränssnitt som används av nummer och alla uttryck.
 * 3: Detta görs för att uttrycken ska kunna behandla nummer och andra uttryck likadant när man
 *    anger vänster och höger sida av uttrycken.
 */
internal interface IValue
{
    /// <summary>
    /// Returns the numeric value of the object.
    /// </summary>
    double? Value { get; }

    /// <summary>
    /// Returns the multiline string representation of the object.
    /// </summary>
    MultilineString Shape { get; }

    /// <summary>
    /// The table representation of all sticks in the object and it's children.
    /// </summary>
    List<List<IStick>> Sticks { get; }
}