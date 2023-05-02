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
        public List<SettingListMenuItem> MenuItems { get; set; } 

        public SettingsWindow()
        {
            MenuItems = new List<SettingListMenuItem>(GetMenuItems());
            DataContext = this;
            InitializeComponent();
        }

        private IEnumerable<SettingListMenuItem> GetMenuItems()
        {
            yield return new SettingListMenuItem(new GeneralSettingPage());
            yield return new SettingListMenuItem(new StepEditorPage());
            yield return new SettingListMenuItem(new AboutMePage());
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (var item in MenuItems)
            {
                if(item.ItemPage is ISavebleSettingItem canSave)
                {
                    if (canSave.IsChanged && !canSave.CanSave)
                        return;
                }
            }
            e.CanExecute = true;
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var item in MenuItems)
            {
                if (item.ItemPage is ISavebleSettingItem canSave)
                {
                    if (canSave.IsChanged)
                        canSave.Save();
                }
            }
            DialogResult = true;
        }
    }
}
