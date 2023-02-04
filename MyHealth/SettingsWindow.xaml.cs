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

        public static RoutedCommand ArrowUp = new RoutedCommand("ArrowUp",typeof(SettingsWindow));
        public static RoutedCommand ArrowDown = new RoutedCommand("ArrowDown",typeof(SettingsWindow));

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ArrowKeys_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lstItems.SelectedIndex == -1)
                return;

            if(ArrowUp == e.Command)
            {
                e.CanExecute = lstItems.SelectedIndex > 0;
            }
            else if (ArrowDown == e.Command)
            {
                e.CanExecute = lstItems.SelectedIndex < lstItems.Items.Count - 1;
            }
        }
        private void ArrowKeys_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ArrowUp == e.Command)
            {
                int index = lstItems.SelectedIndex;
                int target = index - 1;
                object a = lstItems.SelectedItem;
                lstItems.Items.RemoveAt(index);
                lstItems.Items.Insert(target, a);
                lstItems.SelectedIndex = target;
                lstItems.Focus();


            }
            else if (ArrowDown == e.Command)
            {
                int index = lstItems.SelectedIndex;
                int target = index + 1;
                object a = lstItems.SelectedItem;
                lstItems.Items.RemoveAt(index);
                lstItems.Items.Insert(target, a);
                lstItems.SelectedIndex = target;
                lstItems.Focus();
            }
        }

        private void NewOrRemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ApplicationCommands.New == e.Command)
                e.CanExecute = lstItems.Items.Count < 10;
            else if (ApplicationCommands.Delete == e.Command)
                e.CanExecute = lstItems.SelectedIndex != -1;
        }

        private void NewOrRemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ApplicationCommands.New == e.Command)
                lstItems.Items.Add("I`m New Step");
            else if (ApplicationCommands.Delete == e.Command)
                lstItems.Items.RemoveAt(lstItems.SelectedIndex);
        }
    }
}
