// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using System;

    public class MenuItemSelectedEventArgs : EventArgs
    {
        public string SelectedMenuItem { get; set; }

        public MenuItemSelectedEventArgs(string selectedMenuItem)
        {
            SelectedMenuItem = selectedMenuItem;
        }
    }

    public static class MenuItems
    {
        public const string StartGame = "StartGame";
        public const string Options = "Options";
        public const string ExitGame = "ExitGame";
    }
}