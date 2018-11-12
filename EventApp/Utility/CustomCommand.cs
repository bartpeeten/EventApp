using System;
using System.Collections.Generic;
using System.Text;

namespace EventApp.Utility
{
    public class CustomCommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public CustomCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// In UWP we must explicitly trigger the CanExecuteChanged event so that the binding with the control is re-evaluated.
        /// In WPF there is the CommandManager which re-evaluates bindings whenever CanExecuteChanged is raised (Gills implementation).
        /// More info: https://msdn.microsoft.com/en-us/library/system.windows.input.commandmanager
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
