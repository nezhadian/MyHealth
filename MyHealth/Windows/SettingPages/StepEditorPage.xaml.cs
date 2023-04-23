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
    public partial class StepEditorPage : Page,ICanSaveSettingMenuItem
    {

        public ObservableCollection<StepData> StepList = new ObservableCollection<StepData>();

        public bool IsSelected => lstItems.SelectedIndex != -1;
        public StepData SelectedStep => StepList[lstItems.SelectedIndex];

        public bool CanSave
        {
            get
            {
                int effectiveSteps = 0;
                foreach (var item in StepList)
                    if (item.StepType != StepData.StepTypes.FreshStart &&
                        item.StepType != StepData.StepTypes.Seperator)
                        effectiveSteps++;

                return effectiveSteps > 0;
            }
        }

        public bool IsChanged { get; set ; }

        public void Save()
        {
            StepData[] data = new StepData[StepList.Count];
            StepList.CopyTo(data, 0);
            DataAccess.StepDataList = data;
        }

        #region Loading
        bool isInitialized = false;
        public StepEditorPage() => InitializeComponent();
        private void main_Loaded(object sender, RoutedEventArgs e)
        {
            if (isInitialized)
                return;
            lstItems.Items.Clear();
            lstItems.ItemsSource = StepList;

            LoadStepTypes();
            LoadImageListes();
            LoadStepList();

            IsChanged = false;
            isInitialized = true;
        }

        private void LoadStepList()
        {
            SetStepList(DataAccess.StepDataList);
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
            StepData step = new StepData();
            if (IsSelected)
            {
                int index = lstItems.SelectedIndex + 1;
                StepList.Insert(index, step);
                lstItems.SelectedIndex = index;
            }
            else
                StepList.Add(step);
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected;
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int selIndex = lstItems.SelectedIndex;
            StepList.RemoveAt(selIndex);
            lstItems.SelectedIndex = Math.Min(selIndex,lstItems.Items.Count - 1);
            lstItems.Focus();
        }
        #endregion
        #region Arrow Commands
        public static RoutedCommand ArrowUp = new RoutedCommand("ArrowUp",typeof(StepEditorPage));
        public static RoutedCommand ArrowDown = new RoutedCommand("ArrowDown",typeof(StepEditorPage));

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

            StepData selection = SelectedStep;
            StepList.RemoveAt(selectionIndex);
            StepList.Insert(targetIndex, selection);

            lstItems.SelectedIndex = targetIndex;
            lstItems.Focus();
        }
        #endregion

        private void cboStepType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStep.StepType = (StepData.StepTypes)cboStepType.SelectedValue;
            SwitchUI(SelectedStep.StepType);

            OnAnyChanged();
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
            OnAnyChanged();
        }

        private void tscDuration_TextChanged(object sender, RoutedEventArgs e)
        {
            SelectedStep.Duration = tscDuration.TimeSpan;
            OnAnyChanged();
        }

        private void cboImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStep.ImageList = (StepData.ImageListes)cboImageList.SelectedValue;
            OnAnyChanged();
        }

        private void cboTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string prevTag = e.RemovedItems.Count == 0 ? "" : ((ComboBoxItem)e.RemovedItems[0]).Tag.ToString();
            bool isPreviousCustom = prevTag == "";
            bool hasItems = StepList.Count != 0;

            if (isPreviousCustom && hasItems)
            {
                if(AdonisUI.Controls.MessageBoxResult.No == AdonisUI.Controls.MessageBox.Show("Clear Changes ?", "Clear", AdonisUI.Controls.MessageBoxButton.YesNo))
                {
                    cboTemplates.SelectedItem = e.RemovedItems[0];
                    return;
                }
            }
            
            string tag = (((ComboBoxItem)cboTemplates.SelectedItem).Tag ?? "").ToString();
            if (Templates.TemplateDictionary.TryGetValue(tag, out StepData[] template))
            {
                SetStepList(template);
                OnAnyChanged();
            }

        }
        private void SetStepList(StepData[] steps)
        {
            StepList.Clear();
            foreach (StepData item in steps)
            {
                StepList.Add(item);
            }
        }

        bool isUserChangingValues = true;
        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsSelected)
            {
                stckMain.Visibility = Visibility.Hidden;
                return;
            }

            stckMain.Visibility = Visibility.Visible;
            isUserChangingValues = false;
            SetStepDataValuesToUI(SelectedStep);
            isUserChangingValues = true;
        }
        private void SetStepDataValuesToUI(StepData step)
        {
            cboStepType.SelectedValue = step.StepType;
            txtStepName.Text = step.StepName;
            tscDuration.TimeSpan = step.Duration;
            cboImageList.SelectedValue = step.ImageList;
        }

        private void OnAnyChanged()
        {
            lstItems.Items.Refresh();
            if(isUserChangingValues)
                IsChanged = true;
        }

        
    }
}
