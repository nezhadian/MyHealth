using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<StepData> StepList = new ObservableCollection<StepData>();

        public bool IsSelected => lstItems.SelectedIndex != -1;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void main_Loaded(object sender, RoutedEventArgs e)
        {
            lstItems.Items.Clear();
            lstItems.ItemsSource = StepList;

            LoadStepTypes();
        }

        private void LoadStepTypes()
        {
            cboStepType.Items.Clear();
            cboStepType.ItemsSource = Enum.GetValues(typeof(StepData.StepTypes));
        }

        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StepList.Add(new StepData());
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected;
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StepList.RemoveAt(lstItems.SelectedIndex);
        }

        #region Arrow Commands
        public static RoutedCommand ArrowUp = new RoutedCommand("ArrowUp",typeof(SettingsWindow));
        public static RoutedCommand ArrowDown = new RoutedCommand("ArrowDown",typeof(SettingsWindow));

        private void ArrowUp_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected && lstItems.SelectedIndex > 0;
        private void ArrowDown_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected && lstItems.SelectedIndex < StepList.Count - 1;
        
        private void ArrowCommands_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            bool isArrowUp = e.Command == ArrowUp;
            bool isArrowDown = e.Command == ArrowDown;
            if (!(isArrowUp || isArrowDown))
                return;

            int selectionIndex = lstItems.SelectedIndex;
            int targetIndex = isArrowUp ? selectionIndex - 1 : selectionIndex + 1;

            StepData selection = StepList[selectionIndex];
            StepList.RemoveAt(selectionIndex);
            StepList.Insert(targetIndex, selection);

            lstItems.SelectedIndex = targetIndex;
            lstItems.Focus();
        }
        #endregion

        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(cboStepType.SelectedValue.ToString());
        }
    }

    public class StepData
    {
        public enum StepTypes
        {
            WorkTime,FreshStart,ImageSlider
        }
    }
}
