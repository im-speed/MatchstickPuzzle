namespace MatchstickPuzzle.Classes.Menu.CloseActions;
internal class StandardClose : ICloseAction
{
    public List<Action> AdditionalActions { get; set; }

    public StandardClose(params Action[] additionalActions)
    {
        AdditionalActions = additionalActions.ToList();
    }

    public void Close(IMenu menu)
    {
        foreach (Action action in AdditionalActions)
        {
            action();
        }

        menu.Opened = false;
    }
}
