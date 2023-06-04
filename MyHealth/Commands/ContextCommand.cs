using System;
using System.Windows.Input;

namespace MyHealth
{
    public abstract class Command : ICommand
    {
        public abstract void Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
    public abstract class ContextCommand<T> : Command
    {
        protected T Context;

        public ContextCommand(T context)
        {
            Context = context;
        }
        public ContextCommand() { }

        public abstract bool CanExecute(T context, object parameter);
        public abstract void Execute(T context, object parameter);

        public sealed override bool CanExecute(object parameter)
        {
            return CanExecute(Context, parameter);
        }
        public sealed override void Execute(object parameter)
        {
            Execute(Context, parameter);
        }
    }
}

