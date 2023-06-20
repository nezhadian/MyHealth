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
        public bool IsChanged { get; set; }
        public void SetValuesToAppSettings() { }
        public bool CanSave()
        {
            if (tscImageSliderDelay.TimeSpan == TimeSpan.Zero)
                tscImageSliderDelay.TimeSpan = new TimeSpan(0, 0, 5);

            return true;
        }
        public void UndoChanges()
        {
            AppSettings.UndoAll();
        }
        public void Reset()
        {
            IsChanged = false;
        }

        //ctor
        public GeneralSettingPage()
        {
            DataContext = AppSettings.Data;
            InitializeComponent();
        }

        //Open Folder Buttons
        private void OpenImageSliderFolders_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            string directoryPath = App.GetAssetsPath("Images",tag);
            try
            {
                Process.Start("explorer.exe", directoryPath);
                throw new NotImplementedException("this is this is ");
            }
            catch (Exception ex)
            {
                Utils.ErrorMessageBox(ex);
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
            if (Utils.YesNoMessageBoxFromResources("GeneralSettings.Reset"))
            {
                AppSettings.Reset();
            }
        }

        private void Controls_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            IsChanged = true;
        }


    }
}
