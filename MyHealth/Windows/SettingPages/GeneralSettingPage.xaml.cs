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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for GlobalSettingPage.xaml
    /// </summary>
    public partial class GeneralSettingPage : Page,ICanSaveSettingMenuItem
    {
        public bool IsChanged { get; set; } = false;

        public GeneralSettingPage()
        {
            InitializeComponent();
        }
        public bool CanSave => true;
        public void Save()
        {
            Properties.Settings.Default.StartAtStartup = chkStartAtStartup.IsChecked.Value;
            Properties.Settings.Default.Save();
        }

        private void chkStartAtStartup_Click(object sender, RoutedEventArgs e)
        {
            IsChanged = true;
        }

        bool isInitialized = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (isInitialized)
                return;

            chkStartAtStartup.IsChecked = Properties.Settings.Default.StartAtStartup;

            isInitialized = true;
        }
    }
}
