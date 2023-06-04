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
    public partial class AboutMeSettingPage : UserControl
    {
        public AboutMeSettingPage()
        {
            InitializeComponent();
        }

        private void Gmail_Click(object sender, RoutedEventArgs e)
        {
            SocialLinkCardButton socialLink = (SocialLinkCardButton)sender;
            Clipboard.SetText(socialLink.Link.ToString()); ;
            AdonisUI.Controls.MessageBox.Show( "Gmail Address Copied to your Clipboard", "Gmail", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
        }
    }
}
