using System.Windows.Input;
using System;

public class SaveGameCommand : ICommand
{
    private Action<string> _execute;

    public SaveGameCommand(Action<string> execute)
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
        if (parameter is string filePath)
        {
            _execute(filePath);
        }
    }
}
