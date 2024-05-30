using System;
using System.Windows.Input;

namespace MVPTema2EnacheMihai.Commands
{
    public class LoadGameCommand : ICommand
    {
        private Action _execute;

        public LoadGameCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
