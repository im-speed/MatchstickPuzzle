namespace MatchstickPuzzle.Classes.Menu
{
    internal class MenuOption
    {
        public string Text { get; set; }
        public Action Action { get; set; }

        public MenuOption(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}
