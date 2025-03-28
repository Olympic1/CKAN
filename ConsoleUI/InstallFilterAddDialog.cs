using System;

using CKAN.ConsoleUI.Toolkit;

namespace CKAN.ConsoleUI {

    /// <summary>
    /// Popup letting user enter an installation filter
    /// </summary>
    public class InstallFilterAddDialog : ConsoleDialog {

        /// <summary>
        /// Initialize the popup
        /// </summary>
        /// <param name="theme">The visual theme to use to draw the dialog</param>
        public InstallFilterAddDialog(ConsoleTheme theme) : base(theme)
        {
            int l = GetLeft(),
                r = GetRight();
            int t = GetTop(),
                b = t + 4;
            SetDimensions(l, t, r, b);

            manualEntry = new ConsoleField(
                l + 2, b - 2, r - 2
            ) {
                GhostText = () => Properties.Resources.FilterAddGhostText
            };
            AddObject(manualEntry);
            manualEntry.AddTip(Properties.Resources.Enter, Properties.Resources.FilterAddAcceptTip);
            manualEntry.AddBinding(Keys.Enter, sender =>
            {
                choice = manualEntry.Value;
                return false;
            });

            AddTip(Properties.Resources.Esc, Properties.Resources.Cancel);
            AddBinding(Keys.Escape, sender =>
            {
                choice = null;
                return false;
            });

            CenterHeader = () => Properties.Resources.FilterAddTitle;
        }

        /// <summary>
        /// Display the dialog and handle its interaction
        /// </summary>
        /// <param name="process">Function to control the dialog, default is normal user interaction</param>
        /// <returns>
        /// User input
        /// </returns>
        public new string? Run(Action? process = null)
        {
            base.Run(process);
            return choice;
        }

        private readonly ConsoleField manualEntry;
        private          string?      choice;
    }
}
