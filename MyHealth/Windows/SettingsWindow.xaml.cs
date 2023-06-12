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


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var tabItems = tbcPages.Items.OfType<TabItem>();

            foreach (var tabItem in tabItems)
            {
                if(tabItem.Content is ISavebleSettingPage item)
                {
                    if (!item.IsChanged)
                        continue;

                    tabItem.IsSelected = true;

                    if (item.CanSave())
                        item.SetValuesToAppSettings();
                    else
                        return;
                }
            }

            AppSettings.SaveAsync();
            Hide();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
                savebles.ForEach((i) => i.Reset());
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
            var hasChangedItem = savebles.Any((i) => i.IsChanged);

            if (hasChangedItem)
            {
                var isClearAccepted = Utils.YesNoMessageBox("Cancel Settings", "there is Unsaved Changes do you want to clear them?", "Clear All", "Don`t Clear");
                if (isClearAccepted)
                {
                    savebles.ForEach((i) => i.UndoChanges());
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            Hide();
            e.Cancel = true;
        }

    }
}
