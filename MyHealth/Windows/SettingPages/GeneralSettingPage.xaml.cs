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
        public bool CanSave => tscImageSliderDelay.TimeSpan != TimeSpan.Zero;
        public void Save()
        {
            App.StartAtStartup = chkStartAtStartup.IsChecked.Value;
            AppSettings.Data.FreshStartBgColor = clrFreshStartBG.Color;
            AppSettings.Data.ShortBreakBgColor = clrShortBreakBG.Color;
            AppSettings.Data.ImageSliderDelay = tscImageSliderDelay.TimeSpan;
            AppSettings.Save();
        }

        public GeneralSettingPage()
        {
            InitializeComponent();
            LoadSettingValues();
        }

        private void LoadSettingValues()
        {
            clrFreshStartBG.Color = AppSettings.Data.FreshStartBgColor;
            clrShortBreakBG.Color = AppSettings.Data.ShortBreakBgColor;
            tscImageSliderDelay.TimeSpan = AppSettings.Data.ImageSliderDelay;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            chkStartAtStartup.IsChecked = App.StartAtStartup;
        }

        private void OnAnyChanged(object sender, RoutedEventArgs e) => IsChanged = true;

        private void OpenImageSliderFolders_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            string directoryPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Images", tag);
            try
            {
                Process.Start("explorer.exe", directoryPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can`t Open Folder \r\n {ex.Message}");
            }
            
        }

        private void ShowFolderPath_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            string directoryPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Images", tag);
            button.ToolTip = directoryPath;
        }

        private void ResetSettings(object sender, RoutedEventArgs e)
        {
            
            if (AdonisUI.Controls.MessageBoxResult.Yes == AdonisUI.Controls.MessageBox.Show("Are you sure?","Reset Settings", AdonisUI.Controls.MessageBoxButton.YesNo))
            {
                App.ResetAllSettings();
                LoadSettingValues();
                chkStartAtStartup.IsChecked = App.StartAtStartup;
            }
        }
    }
}
