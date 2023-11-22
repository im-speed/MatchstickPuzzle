using MatchstickPuzzle.Classes.ValueTypes.MultilineString;

namespace MatchstickPuzzle.Classes.Game
{
    internal class Number : IValue
    {
        private readonly List<Digit> digits;

        public double? Value
        {
            get
            {
                string number = string.Join("", digits.Select(digit => digit.Value.ToString()));

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
                    multilineString = multilineString.CombineHorizontally(digit.Shape);
                }

                return multilineString;
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
}
