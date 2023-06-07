using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class GeneralSettingPage : UserControl, ISavebleSettingPage
    {
        //Implement ISavebleSettingPage
        public bool IsChanged { get; set; } = false;
        public bool CanSave => tscImageSliderDelay.TimeSpan != TimeSpan.Zero;
        public void Save()
        {
            App.StartAtStartup = chkStartAtStartup.IsChecked.Value;
            //AppSettings.Data.FreshStartBgColor = cselFreshStart.Color;
            AppSettings.Data.ShortBreakBgColor = cselShortBreak.Color;
            AppSettings.Data.ImageSliderDelay = tscImageSliderDelay.TimeSpan;
            AppSettings.Save();
        }

        //ctor
        public GeneralSettingPage()
        {
            DataContext = AppSettings.Data;
            InitializeComponent();
        }

        //Loading
        private void OnInitialized(object sender, EventArgs e)
        {
            LoadSettingValuesFrom(AppSettings.Data);
            chkStartAtStartup.IsChecked = App.StartAtStartup;
            IsChanged = false;
        }
        private void LoadSettingValuesFrom(MyHealthSettings settings)
        {
            //cselFreshStart.Color = settings.FreshStartBgColor;
            cselShortBreak.Color = settings.ShortBreakBgColor;
            tscImageSliderDelay.TimeSpan = settings.ImageSliderDelay;
        }

        //Detect Changes
        private void OnAnyChanged(object sender, RoutedEventArgs e) => IsChanged = true;

        //Open Folder Buttons
        private void OpenImageSliderFolders_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            string directoryPath = App.GetAssetsPath("Images",tag);
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

        //Reset Settings Button
        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            if (AdonisUI.Controls.MessageBoxResult.Yes == AdonisUI.Controls.MessageBox.Show("Are you sure?", "Reset Settings", AdonisUI.Controls.MessageBoxButton.YesNo))
            {
                AppSettings.Reset();
                IsChanged = true;
            }
        }


    }
}
