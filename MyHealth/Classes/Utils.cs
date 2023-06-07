namespace MyHealth
{
    internal class Utils
    {
        public static bool YesNoMessageBox(string caption, string message)
        {
            return AdonisUI.Controls.MessageBoxResult.Yes == AdonisUI.Controls.MessageBox.Show(message, caption, AdonisUI.Controls.MessageBoxButton.YesNo);
        }
    }
}