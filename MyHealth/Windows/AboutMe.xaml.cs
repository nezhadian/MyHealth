using System;
using System.Collections.Generic;
using System.Text;
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
    public partial class AboutMe : Window
    {
        public AboutMe()
        {
            InitializeComponent();
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Clipboard.SetText(button.ToolTip.ToString());
        }
    }
}
