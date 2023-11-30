using MatchstickPuzzle.Classes.Game.Sticks;
using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game;

internal class Number : IValue
{
    /* 1: Inkapsling / Informationsgömning
     * 2: Här göms fältet digits undan och istället får man värderna genom Number:s egenskaper. 
     * 3: Detta görs eftersom det inte finns någon anledning att hämta värderna direkt från en Digit 
     *    utan de bör behandlas som om de satt íhop i ett tal, vilket är vad denna klass är till för.
     */
    private readonly List<Digit> digits;

    public double? Value
    {
        get
        {
            if (digits.Exists((digit) => digit.Value == null))
            {
                return null;
            }

            string number = string.Join("", digits.Select(digit => digit.Value == -1 ? "" : digit.Value.ToString()));

            if (int.TryParse(number, out int value))
            {
                return value;
            }

            return null;
        }
    }

    public MultilineString Shape
    {
        get
        {
            if (digits.Count == 0)
            {
                return new("");
            }

            MultilineString multilineString = digits[0].Shape;

            foreach (Digit digit in digits.Skip(1))
            {
                multilineString = multilineString.CombineHorizontally(digit.Shape, " ");
            }

            return multilineString;
        }
    }

    public List<List<IStick>> Sticks
    {
        get
        {
            List<List<IStick>> sticks = new();

            foreach (Digit digit in digits)
            {
                if (sticks.Count == 0)
                {
                    sticks.AddRange(digit.Sticks);
                    continue;
                }

                for (int i = 0; i < digit.Sticks.Count; i++)
                {
                    sticks[i].AddRange(digit.Sticks[i]);
                }
            }

            return sticks;
        }
    }

    public Number(int number)
    {
        List<char> charDigits = number.ToString().ToCharArray().ToList();

        digits = new();
        foreach (char digit in charDigits)
        {
            digits.Add(new(digit));
        }
    }
}
