namespace MyHealth
{
    public abstract class ViewModelCommands<T> : Command
    {
        protected T Context;

        public ViewModelCommands(T context)
        {
            Context = context;
        }
        public ViewModelCommands() { }

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

