using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
            var hasChangedItem = savebles.Any(i => i.IsChanged);
            var hasUnSavbleItem = savebles.Any(i => i.IsChanged && !i.CanSave());

            e.CanExecute = !hasChangedItem || !hasUnSavbleItem;
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
            savebles.ForEach(i =>
            {
                if (i.IsChanged)
                    i.SetValuesToAppSettings();
            });
            AppSettings.Save();
            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == true)
                return;

            var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
            var hasChangedItem = savebles.Any((i) => i.IsChanged);

            if (hasChangedItem)
            {
                if (Utils.YesNoMessageBox("Cancel Settings", "there is Unsaved Changes do you want to clear them?"))
                {
                    AppSettings.UndoAll();
                    DialogResult = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult = false;
            }
        }
    }
}
