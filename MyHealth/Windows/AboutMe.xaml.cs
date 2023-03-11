using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for AboutMe.xaml
    /// </summary>
    public partial class AboutMe : Page
    {
        public AboutMe()
        {
            InitializeComponent();
        }
        private void OpenLinkCardButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (CardButton)sender;
            Process.Start(new ProcessStartInfo(button.ToolTip.ToString()) { UseShellExecute = true });
        }

        private void CopyGmail_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("yasin.ebr.nezh@gmail.com");
            MainWindow.TaskBarIcon.ShowBalloonTip("Gmail", "Gmail Copied", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
        }
    }
}
