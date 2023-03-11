using System;
using System.Collections.Generic;
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
    public partial class AboutMe : Window
    {
        public AboutMe()
        {
            InitializeComponent();
        }

        Style btnPreviusStyle;
        Button btnHasCopyStyle = null;

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if(btnHasCopyStyle != null)
            {
                btnHasCopyStyle.Style = btnPreviusStyle;
            }
            var button = (CardButton)sender;
            btnPreviusStyle = button.Style;
            btnHasCopyStyle = button;
            button.Style = (Style)FindResource("AboutMe.Copied");
            Clipboard.SetText(button.ToolTip.ToString());
        }
    }
}
