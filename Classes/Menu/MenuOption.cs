namespace MatchstickPuzzle.Classes.Menu;

internal class MenuOption
{
    /// <summary>
    /// The string that will be written in the menu.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// The action to run when this option is chosen.
    /// </summary>
    public Action Action { get; set; }

    /// <param name="text">The string that will be written in the menu.</param>
    /// <param name="action">The action to run when this option is chosen.</param>
    public MenuOption(string text, Action action)
    {
        Text = text;
        Action = action;
    }
}
