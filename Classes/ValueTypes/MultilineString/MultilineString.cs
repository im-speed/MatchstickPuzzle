namespace MatchstickPuzzle.Classes.ValueTypes.MultilineString;

internal class MultilineString
{
    private List<string> _value;

    /// <summary>
    /// The multiline string represented as a normal string.
    /// </summary>
    public string Value
    {
        get
            => string.Join('\n', _value);
        set
            => _value = value.Split('\n').ToList();
    }

    /// <summary>
    /// The current amount of rows.
    /// </summary>
    public int Rows
        => _value.Count;

    /// <summary>
    /// The length of the current longest row.
    /// </summary>
    public int Width
        => _value.Max((row) => row.Length);

    /* 1: Overloading av konstruktorer
     * 2: Här överlagras konstruktorn ifall man inte ger något initialt värde.
     * 3: Syftet är helt enkelt att om man vill börja med en tom sträng finns det ingen anledning att behöva ange ett värde.
     */

    /// <summary>
    /// Create an empty multiline string.
    /// </summary>
    public MultilineString()
        : this("")
    {
    }

    public MultilineString(string value)
    {
        _value = value.Split('\n').ToList();
    }

    /* 1: Overloading av instansmetoder
     * 2: Här överlagras både CombineHorizontally och CombineVertically för olika kombinationer av parametrar.
     * 3: Detta görs då det finns många situationer när man kanske inte behöver ange allt. T.ex. vill man inte
     *    alltid separera strängerna eller så vet man att de har lika många rader / är lika breda och då inte behöver ange någon
     *    alignment.
     */

    /// <inheritdoc cref="CombineHorizontally(MultilineString, VerticalAlignment, string)"/>
    public MultilineString CombineHorizontally(MultilineString other)
        => CombineHorizontally(other, VerticalAlignment.Bottom);

    /// <inheritdoc cref="CombineHorizontally(MultilineString, VerticalAlignment, string)"/>
    public MultilineString CombineHorizontally(MultilineString other, VerticalAlignment alignment)
        => CombineHorizontally(other, alignment, "");

    /// <inheritdoc cref="CombineHorizontally(MultilineString, VerticalAlignment, string)"/>
    public MultilineString CombineHorizontally(MultilineString other, string separator)
        => CombineHorizontally(other, VerticalAlignment.Bottom, separator);

    /// <summary>
    /// Combines this multiline string with another horizontally without altering either of them.
    /// </summary>
    /// <param name="other">The multiline string to combine with.</param>
    /// <param name="alignment">The alignment to use when aligning the multiline string with the fewest rows.</param>
    /// <param name="separator">What string to put between the multiline strings on each row.</param>
    /// <returns>A new combined muliline string.</returns>
    public MultilineString CombineHorizontally(MultilineString other, VerticalAlignment alignment, string separator)
    {
        MultilineString newString;

        if (Rows > other.Rows)
        {
            other = other.AlignVertically(alignment, Rows);
            newString = AlignHorizontally(HorizontalAlignment.Left, Width);
        }
        else
        {
            newString = AlignVertically(alignment, other.Rows)
                .AlignHorizontally(HorizontalAlignment.Left, Width);
        }

        for (int i = 0; i < Rows; i++)
        {
            newString._value[i] += $"{separator}{other._value[i]}";
        }

        return newString;
    }

    /// <inheritdoc cref="CombineVertically(MultilineString, HorizontalAlignment, int)"/>
    public MultilineString CombineVertically(MultilineString other)
        => CombineVertically(other, HorizontalAlignment.Left);

    /// <inheritdoc cref="CombineVertically(MultilineString, HorizontalAlignment, int)"/>
    public MultilineString CombineVertically(MultilineString other, HorizontalAlignment alignment)
        => CombineVertically(other, alignment, 0);

    /// <inheritdoc cref="CombineVertically(MultilineString, HorizontalAlignment, int)"/>
    public MultilineString CombineVertically(MultilineString other, int separatingRows)
        => CombineVertically(other, HorizontalAlignment.Left, separatingRows);

    /// <summary>
    /// Combines this multiline string with another vertically without altering either of them.
    /// </summary>
    /// <param name="other">The multiline string to combine with.</param>
    /// <param name="alignment">The alignment to use when aligning the multiline string with the smallest width.</param>
    /// <param name="separatingRows">How many empty rows to put between the multiline strings.</param>
    /// <returns>A new combined muliline string.</returns>
    public MultilineString CombineVertically(MultilineString other, HorizontalAlignment alignment, int separatingRows)
    {
        MultilineString newString;

        if (Width > other.Width)
        {
            other = other.AlignHorizontally(alignment, Width);
            newString = new(Value);
        }
        else
        {
            newString = AlignHorizontally(alignment, other.Width);
        }

        for (int i = 0; i < separatingRows; i++)
        {
            newString._value.Add("");
        }

        newString._value.AddRange(other._value);

        return newString;
    }

    /// <summary>
    /// Creates a new multiline string that is aligned as specified with empty rows.
    /// </summary>
    /// <param name="alignment">How to align the multiline string.</param>
    /// <param name="rows">How many rows the new multiline string should have. Can not be lower than the current rows of the 
    /// multiline string.</param>
    /// <returns>The new aligned multiline string.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public MultilineString AlignVertically(VerticalAlignment alignment, int rows)
    {
        if (rows < Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), "Can not align to less rows than the string takes up.");
        }

        MultilineString newString = new(Value);
        int difference = rows - Rows;

        if (alignment == VerticalAlignment.Top)
        {
            for (int i = 0; i < difference; i++)
            {
                newString._value.Add("");
            }
        }

        else if (alignment == VerticalAlignment.Middle)
        {

            for (int i = 0; i < difference / 2 + difference % 2; i++)
            {
                newString._value.Insert(0, "");
            }

            for (int i = 0; i < difference / 2; i++)
            {
                newString._value.Add("");
            }
        }

        else if (alignment == VerticalAlignment.Bottom)
        {
            for (int i = 0; i < difference; i++)
            {
                newString._value.Insert(0, "");
            }
        }

        return newString;
    }

    /// <summary>
    /// Creates a new multiline string that is aligned as specified with spaces to make every row equal length.
    /// </summary>
    /// <param name="alignment">How to align the multiline string.</param>
    /// <param name="width">How long each row should be. Can not be smaller than the current width of the multiline string.</param>
    /// <returns>The new aligned multiline string.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public MultilineString AlignHorizontally(HorizontalAlignment alignment, int width)
    {
        if (width < Width)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Can not align to a smaller width than the string takes up.");
        }

        MultilineString newString = new(Value);

        if (alignment == HorizontalAlignment.Left)
        {
            for (int i = 0; i < Rows; i++)
            {
                newString._value[i] = _value[i].PadRight(width);
            }
        }

        else if (alignment == HorizontalAlignment.Center)
        {
            for (int i = 0; i < Rows; i++)
            {
                int difference = width - _value[i].Length;
                newString._value[i] = _value[i].PadLeft(width - difference / 2);
                newString._value[i] = newString._value[i].PadRight(width);
            }
        }

        else if (alignment == HorizontalAlignment.Right)
        {
            for (int i = 0; i < Rows; i++)
            {
                newString._value[i] = _value[i].PadLeft(width);
            }
        }

        return newString;
    }

    /// <summary>
    /// Returns Value.
    /// </summary>
    public override string ToString()
    {
        return Value;
    }

    /// <inheritdoc cref="Join(IEnumerable{MultilineString}, VerticalAlignment, string)"/>
    public static MultilineString Join(IEnumerable<MultilineString> values)
        => Join(values, VerticalAlignment.Bottom);

    /// <inheritdoc cref="Join(IEnumerable{MultilineString}, VerticalAlignment, string)"/>
    public static MultilineString Join(IEnumerable<MultilineString> values, VerticalAlignment alignment)
        => Join(values, alignment, " ");

    /// <inheritdoc cref="Join(IEnumerable{MultilineString}, VerticalAlignment, string)"/>
    public static MultilineString Join(IEnumerable<MultilineString> values, string separator)
        => Join(values, VerticalAlignment.Bottom, separator);

    /// <summary>
    /// Combines all the values horizontally.
    /// </summary>
    /// <param name="values">A collection of all multiline strings to combine.</param>
    /// <inheritdoc cref="CombineHorizontally(MultilineString, VerticalAlignment, string)"/>
    public static MultilineString Join(IEnumerable<MultilineString> values, VerticalAlignment alignment, string separator)
    {
        if (!values.Any())
        {
            return new();
        }

        MultilineString result = values.First();

        foreach (MultilineString value in values.Skip(1))
        {
            result = result.CombineHorizontally(value, alignment, separator);
        }

        return result;
    }
}
