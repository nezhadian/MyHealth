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
        public bool CanSave => true;
        public void Save()
        {
            App.StartAtStartup = chkStartAtStartup.IsChecked.Value;
            Properties.Settings.Default.FreshStartBgColor = clrFreshStartBG.Color;
            Properties.Settings.Default.Save();
        }

        bool isInitialized = false;

        public GeneralSettingPage() => InitializeComponent();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            chkStartAtStartup.IsChecked = App.StartAtStartup;

            if (isInitialized)
                return;

            clrFreshStartBG.Color = Properties.Settings.Default.FreshStartBgColor;

            isInitialized = true;
        }

        private void OnAnyChanged(object sender, RoutedEventArgs e) => IsChanged = true;

    }
}
