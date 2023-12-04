using MatchstickPuzzle.Classes.Menu.CloseActions;
using MatchstickPuzzle.Classes.Menu.Styles;

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
    List<Option> Options { get; }

    /// <summary>
    /// The message shown above all menu options.
    /// </summary>
    string? Message { get; set; }

    /* 1: Objektkomposition
     * 2: Alla menyer har en CloseAction som kan kallas med CloseAction.Close(this) för att stänga
     *    menyn och eventuellt göra ytterligare funktioner som att fråga ifall man vill stänga
     *    menyn först.
     * 3: Syftet är väl standarden för objektkomposition, att man slipper göra separata menyklasser
     *    för dessa olika funktioner, eftersom menyena har ett sätt att stänga sig istället för att
     *    de är ett sätt att stänga sig.
     */

    /* 1: Beroendeinjektion
     * 2: CloseAction är beroendeinjektion då den bara behöver implementera gränssnittet och sedan
     *    används CloseAction.Close() utan att menyn vet vad den gör.
     * 3: Anledningen till att gör så är att menyn inte behöver mer än att sätta Opened = false.
     *    Men man kan vilja ha flera sätt att stänga menyn som gör annat innan det, som inte skulle
     *    vara praktiskt att ange i t.ex. boolska egenskaper.
     */

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

    /// <summary>
    /// Sets the style for all menu options.
    /// </summary>
    IOptionStyle OptionStyle { get; set; }

    /// <summary>
    /// Opens the menu.
    /// </summary>
    void Open();
}
