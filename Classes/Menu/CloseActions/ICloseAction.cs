namespace MatchstickPuzzle.Classes.Menu.CloseActions;
internal interface ICloseAction
{
    List<Action> AdditionalActions { get; set; }

    void Close(IMenu menu);
}
