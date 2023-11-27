namespace MatchstickPuzzle.Classes.Menu.CloseActions;
internal class ConfirmClose : ICloseAction
{
    public List<Action> AdditionalActions { get; set; }
    public string ConfirmMessage { get; set; } = "Are you sure you wish to exit?";

    public ConfirmClose(params Action[] additionalActions)
    {
        AdditionalActions = additionalActions.ToList();
    }

    public void Close(IMenu menu)
    {
        if (ListMenu.YesOrNo(ConfirmMessage))
        {
            foreach (Action action in AdditionalActions)
            {
                action();
            }

            menu.Opened = false;
        }
    }
}
