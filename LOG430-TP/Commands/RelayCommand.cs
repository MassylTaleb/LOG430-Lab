using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LOG430_TP.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _Action;
        private readonly Func<bool> _CanExecute;

        public RelayCommand(Action action)
        {
            _Action = action;
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            _Action = action;
            _CanExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _Action();
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_CanExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_CanExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
    }
}
