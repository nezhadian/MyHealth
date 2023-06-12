using AdonisUI.Controls;
using System.Windows.Input;

namespace MyHealth
{
    internal class Utils
    {
        public static bool YesNoMessageBox(string caption, string message,string yesLabel = "Yes",string noLabel = "No")
        {
            MessageBoxModel model = new MessageBoxModel();
            model.Buttons = new MessageBoxButtonModel[]
            {
                new MessageBoxButtonModel(yesLabel,MessageBoxResult.Yes),
                new MessageBoxButtonModel(noLabel,MessageBoxResult.No),
            };

            model.Text = message;
            model.Caption = caption;
            
            return MessageBoxResult.Yes == MessageBox.Show(model);
        }

        public static bool RaiseCommand(ICommand command,object param)
        {
            if (command.CanExecute(param))
            {
                command.Execute(param);
                return true;
            }
            return false;
        }
    }
}