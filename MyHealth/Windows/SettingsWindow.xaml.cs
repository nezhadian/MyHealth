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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        List<SettingListMenuItem> MenuItems = new List<SettingListMenuItem>()
        {
            new SettingListMenuItem("General",new GeneralSettingPage()),
            new SettingListMenuItem("Step Editor",new StepEditorPage()),
            new SettingListMenuItem("About Me",new AboutMePage()),
        };

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            lstMenu.Items.Clear();
            lstMenu.ItemsSource = MenuItems;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (var item in MenuItems)
            {
                if(item.ItemPage is ICanSaveSettingMenuItem)
                {
                    ICanSaveSettingMenuItem canSave = (ICanSaveSettingMenuItem)item.ItemPage;
                    if (canSave.IsChanged &&!canSave.CanSave)
                        return;
                }
            }
            e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in MenuItems)
            {
                if (item.ItemPage is ICanSaveSettingMenuItem)
                {
                    ICanSaveSettingMenuItem canSave = (ICanSaveSettingMenuItem)item.ItemPage;
                    if (canSave.IsChanged)
                        ((ICanSaveSettingMenuItem)item.ItemPage).Save();
                }
            }
            DialogResult = true;
        }
    }
}
