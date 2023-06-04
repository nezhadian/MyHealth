using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MyHealth
{
    public class OpenLinkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public static OpenLinkCommand Instance;

        public bool CanExecute(object parameter)
        {
            if (parameter is Uri)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            Process.Start(new ProcessStartInfo(parameter.ToString()) { UseShellExecute = true });
        }

        static OpenLinkCommand()
        {
            Instance = new OpenLinkCommand();
        }
    }
}
