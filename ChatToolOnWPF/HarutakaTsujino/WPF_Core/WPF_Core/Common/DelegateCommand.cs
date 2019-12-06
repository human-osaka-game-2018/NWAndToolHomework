using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WPF_Core.Common
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action onExecute)
        {
            OnExecute = onExecute;
        }

        public DelegateCommand(Action onExecute, Func<bool> onCanExecute)
        {
            OnExecute = onExecute;
            OnCanExecute = onCanExecute;
        }

        public bool CanExecute()
        {
            if (OnCanExecute == null)
            {
                return true;
            }
            else
            {
                return OnCanExecute();
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public void Execute()
        {
            OnExecute();
        }

        public void Execute(object parameter)
        {
            Execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private Action OnExecute { get; set; }

        private Func<bool> onCanExecute;
        private Func<bool> OnCanExecute
        {
            get => onCanExecute;
            set 
            {
                onCanExecute = value;

                RaiseCanExecuteChanged();
            }
        }
    }
}
