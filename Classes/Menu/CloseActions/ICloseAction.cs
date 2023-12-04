namespace MatchstickPuzzle.Classes.Menu.CloseActions;
internal interface ICloseAction
{
    /// <summary>
    /// Additional actions to be run before closing.
    /// </summary>
    List<Action> AdditionalActions { get; set; }

    /// <summary>
    /// Run when closing a menu.
    /// </summary>
    /// <param name="menu">The menu to close</param>
    void Close(IMenu menu);

    /// <summary>
    /// Run when closing a menu without user input.
    /// </summary>
    /// <param name="menu">The menu to close</param>
    void QuickClose(IMenu menu);
}
