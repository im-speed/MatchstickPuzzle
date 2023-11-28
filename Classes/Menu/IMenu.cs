using MatchstickPuzzle.Classes.Menu.CloseActions;

namespace MatchstickPuzzle.Classes.Menu;
internal interface IMenu
{
    /// <summary>
    /// True when the menu is open.
    /// </summary>
    bool Opened { get; set; }

    /// <summary>
    /// All menu options.
    /// </summary>
    List<MenuOption> Options { get; }

    /// <summary>
    /// The message shown above all menu options.
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// What to do when trying to close the menu.
    /// </summary>
    ICloseAction CloseAction { get; set; }

    /// <summary>
    /// If true the user can press escape to call the close action.
    /// </summary>
    bool CloseWithEscape { get; set; }

    /// <summary>
    /// If true the menu will close after the user chooses any menu option.
    /// </summary>
    bool CloseAfterAction { get; set; }
    string OptionPrefix { get; set; }
    string SelectedPrefix { get; set; }
    string OptionSuffix { get; set; }
    string SelectedSuffix { get; set; }

    /// <summary>
    /// Opens the menu.
    /// </summary>
    void Open();
}
