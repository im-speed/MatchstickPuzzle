namespace MatchstickPuzzle.Classes.Menu.CloseActions;
/// <summary>
/// Asks the user if they want to close the menu and if they say yes runs the additional actions and closes the menu.
/// </summary>
internal class ConfirmClose : ICloseAction
{
    public List<Action> AdditionalActions { get; set; }

    /// <summary>
    /// The yes or no question asked when trying to close the menu.
    /// </summary>
    public string ConfirmMessage { get; set; } = "Are you sure you wish to exit?";

    public ConfirmClose(params Action[] additionalActions)
    {
        AdditionalActions = additionalActions.ToList();
    }

    public void Close(IMenu menu)
    {
        if (ListMenu.YesOrNo(ConfirmMessage))
        {
            QuickClose(menu);
        }
    }

    public void QuickClose(IMenu menu)
    {
        foreach (Action action in AdditionalActions)
        {
            action();
        }

        menu.Opened = false;
    }
}
