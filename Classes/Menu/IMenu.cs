using MatchstickPuzzle.Classes.Menu.CloseActions;

namespace MatchstickPuzzle.Classes.Menu;
internal interface IMenu
{
    bool Opened { get; set; }
    List<MenuOption> Options { get; }
    string? Message { get; set; }
    ICloseAction CloseAction { get; set; }
    bool CloseWithEscape { get; set; }
    bool CloseAfterAction { get; set; }
    string OptionPrefix { get; set; }
    string SelectedPrefix { get; set; }
    string OptionSuffix { get; set; }
    string SelectedSuffix { get; set; }

    void Open();
}
