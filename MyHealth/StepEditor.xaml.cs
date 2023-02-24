using System;
using System.Collections;
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
    public partial class StepEditor : Window
    {

        public ObservableCollection<StepData> StepList = new ObservableCollection<StepData>();

        public bool IsSelected => lstItems.SelectedIndex != -1;

        public StepData SelectedStep => StepList[lstItems.SelectedIndex];

        public StepEditor()
        {
            InitializeComponent();
        }

        #region Loading
        private void main_Loaded(object sender, RoutedEventArgs e)
        {
            lstItems.Items.Clear();
            lstItems.ItemsSource = StepList;

            LoadStepTypes();
            LoadImageListes();
            LoadStepList();
        }

        private void LoadStepList()
        {
            AddRange(DataAccess.StepDataList);
        }

        private void LoadImageListes()
        {
            cboImageList.Items.Clear();
            cboImageList.ItemsSource = Enum.GetValues(typeof(StepData.ImageListes));
        }
        private void LoadStepTypes()
        {
            cboStepType.Items.Clear();
            cboStepType.ItemsSource = Enum.GetValues(typeof(StepData.StepTypes));
        }
        #endregion
        #region Commands
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StepList.Add(new StepData());
            lstItems.SelectedIndex = StepList.Count - 1;
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected;
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int selIndex = lstItems.SelectedIndex;
            StepList.RemoveAt(selIndex);
            lstItems.SelectedIndex = Math.Min(selIndex,lstItems.Items.Count - 1);
            lstItems.Focus();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int effectiveSteps = 0;
            foreach (var item in StepList)
                if (item.StepType != StepData.StepTypes.FreshStart &&
                    item.StepType != StepData.StepTypes.Seperator)
                    effectiveSteps++;

            e.CanExecute = effectiveSteps > 0;
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StepData[] data = new StepData[StepList.Count];
            StepList.CopyTo(data, 0);
            DataAccess.StepDataList = data;

            DialogResult = true;
        }
        #endregion
        #region Arrow Commands
        public static RoutedCommand ArrowUp = new RoutedCommand("ArrowUp",typeof(StepEditor));
        public static RoutedCommand ArrowDown = new RoutedCommand("ArrowDown",typeof(StepEditor));

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

        private void cboStepType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStep.StepType = (StepData.StepTypes)cboStepType.SelectedValue;
            lstItems.Items.Refresh();

            SwitchUI(SelectedStep.StepType);
        }
        private void SwitchUI(StepData.StepTypes stepType)
        {
            grdDuration.Visibility =
                grdImageListes.Visibility =
                Visibility.Collapsed;

            switch (stepType)
            {
                case StepData.StepTypes.WorkTime:
                case StepData.StepTypes.ShortBreak:
                    grdDuration.Visibility = Visibility.Visible;
                    break;
                case StepData.StepTypes.ImageSlider:
                    grdDuration.Visibility = Visibility.Visible;
                    grdImageListes.Visibility = Visibility.Visible;
                    break;

            }
        }

        private void txtStepName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedStep.StepName = txtStepName.Text;
            lstItems.Items.Refresh();
        }

        private void tscDuration_TextChanged(object sender, RoutedEventArgs e)
        {
            SelectedStep.Duration = tscDuration.TimeSpan;
            lstItems.Items.Refresh();
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsSelected)
            {
                stckMain.Visibility = Visibility.Hidden;
                return;
            }

            stckMain.Visibility = Visibility.Visible;

            SetStepDataValuesToUI(SelectedStep);
        }
        private void SetStepDataValuesToUI(StepData step)
        {
            cboStepType.SelectedValue = step.StepType;
            txtStepName.Text = step.StepName;
            tscDuration.TimeSpan = step.Duration;
            cboImageList.SelectedValue = step.ImageList;
        }

        private void cboImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStep.ImageList = (StepData.ImageListes)cboImageList.SelectedValue;
            lstItems.Items.Refresh();
        }

        private void cboTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tag = ((ListBoxItem)cboTemplates.SelectedItem).Tag.ToString();
            string prev = e.RemovedItems.Count > 0 ? ((ListBoxItem)e.RemovedItems[0]).Tag.ToString() : "";
            if(Templates.TemplateDictionary.TryGetValue(tag,out StepData[] template))
            {
                var resault = prev == "" && StepList.Count > 0? MessageBox.Show("Clear Changes ?", "Clear", MessageBoxButton.YesNo) : MessageBoxResult.Yes;
                if (resault == MessageBoxResult.Yes)
                {
                    AddRange(template);
                }

            }
        }
        private void AddRange(StepData[] steps)
        {
            StepList.Clear();
            foreach (StepData item in steps)
            {
                StepList.Add(item);
            }
        }



    }
}
