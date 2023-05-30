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
            var isSavebleItemFound = savebles.Any(i => i.IsChanged && i.CanSave);

            e.CanExecute = isSavebleItemFound;
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var savebles = tbcPages.Items.OfType<TabItem>().Select(i => i.Content).OfType<ISavebleSettingPage>().ToList();
            savebles.ForEach(i =>
            {
                if (i.CanSave)
                    i.Save();
            });

            DialogResult = true;
        }
    }
}
