using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Yusar.Client.ViewModels
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object param);
    }


    public class AsyncCommand : IAsyncCommand
    {
        private bool _isExecuting;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        private Dispatcher Dispatcher { get; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            Dispatcher = Application.Current.Dispatcher;
        }

        private void InvalidateRequerySuggested()
        {
            if (Dispatcher.CheckAccess())
                CommandManager.InvalidateRequerySuggested();
            else
                Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        public bool CanExecute(object parameter) => !_isExecuting && (_canExecute == null || _canExecute(parameter));

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    InvalidateRequerySuggested();
                    Task task = new Task(s => _execute(s), parameter);
                    task.Start();
                    await task;
                }
                finally
                {
                    _isExecuting = false;
                    InvalidateRequerySuggested();
                }
            }
        }
        public void Execute(object parameter) => _ = ExecuteAsync(parameter);
    }
}
