using System;
using System.Windows.Input;
using MVPTema2EnacheMihai.Commands;

namespace MVPTema2EnacheMihai.ViewModels
{
    public class WinnerPopupViewModel
    {
        public string WinnerMessage { get; }

        // Define an event to notify about restart
        public event EventHandler RestartRequested;

        public ICommand RestartCommand { get; }

        public WinnerPopupViewModel(string winnerMessage)
        {
            WinnerMessage = winnerMessage;
            RestartCommand = new RestartCommand(Restart);
        }

        private void Restart()
        {
            // Raise the RestartRequested event to notify the parent view model
            RestartRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
