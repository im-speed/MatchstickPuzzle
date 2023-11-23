namespace MatchstickPuzzle.Classes.Menu;

internal interface IMenu
{
    bool Opened { get; set; }

    void Open() { }
    void Close()
    {
        Opened = false;
    }
}
