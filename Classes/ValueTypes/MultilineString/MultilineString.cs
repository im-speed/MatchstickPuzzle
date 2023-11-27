namespace MatchstickPuzzle.Classes.ValueTypes.MultilineString;

internal class MultilineString
{
    private List<string> _value;

    public string Value
    {
        get
            => string.Join('\n', _value);
        set
            => _value = value.Split('\n').ToList();
    }

    public int Rows
        => _value.Count;

    public int Width
        => _value.Max((row) => row.Length);

    public MultilineString()
        : this("")
    {
    }

    public MultilineString(string value)
    {
        _value = value.Split('\n').ToList();
    }

    public MultilineString CombineHorizontally(MultilineString other)
        => CombineHorizontally(other, VerticalAlignment.Bottom);

    public MultilineString CombineHorizontally(MultilineString other, VerticalAlignment alignment)
        => CombineHorizontally(other, alignment, " ");

    public MultilineString CombineHorizontally(MultilineString other, string separator)
        => CombineHorizontally(other, VerticalAlignment.Bottom, separator);

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

    public MultilineString CombineVertically(MultilineString other)
        => CombineVertically(other, HorizontalAlignment.Left);

    public MultilineString CombineVertically(MultilineString other, HorizontalAlignment alignment)
        => CombineVertically(other, alignment, 0);

    public MultilineString CombineVertically(MultilineString other, int separatingRows)
        => CombineVertically(other, HorizontalAlignment.Left, separatingRows);

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

    public override string ToString()
    {
        return Value;
    }

    public static MultilineString Join(IEnumerable<MultilineString> values)
        => Join(values, VerticalAlignment.Bottom);

    public static MultilineString Join(IEnumerable<MultilineString> values, VerticalAlignment alignment)
        => Join(values, alignment, " ");

    public static MultilineString Join(IEnumerable<MultilineString> values, string separator)
        => Join(values, VerticalAlignment.Bottom, separator);

    public static MultilineString Join(IEnumerable<MultilineString> values, VerticalAlignment alignment, string seperator)
    {
        if (values.Count() == 0)
        {
            return new();
        }

        MultilineString result = values.First();

        foreach (MultilineString value in values.Skip(1))
        {
            result = result.CombineHorizontally(value, alignment, seperator);
        }

        return result;
    }
}
