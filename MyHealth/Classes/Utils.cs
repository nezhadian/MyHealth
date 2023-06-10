using AdonisUI.Controls;
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
    }
}