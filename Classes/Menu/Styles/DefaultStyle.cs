namespace MatchstickPuzzle.Classes.Menu.Styles;
internal class DefaultStyle : IOptionStyle
{
    public string Prefix { get; set; }
    public string SelectedPrefix { get; set; }
    public string Suffix { get; set; }
    public string SelectedSuffix { get; set; }

    public DefaultStyle(string prefix, string selectedPrefix, string suffix, string selectedSuffix)
    {
        Prefix = prefix;
        SelectedPrefix = selectedPrefix;
        Suffix = suffix;
        SelectedSuffix = selectedSuffix;
    }

    public string Style(Option option, bool selected)
    {
        return $"{(selected ? SelectedPrefix : Prefix)}{option.Text}{(selected ? SelectedSuffix : Suffix)}";
    }
}
