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
using System.Linq;

namespace MyHealth
{
    public partial class StepsEditorSettingPage : UserControl,ISavebleSettingPage
    {
        //ISavebleSettingItem Implementation
        public bool CanSave
        {
            get
            {
                int effectiveSteps = StepList.Count((item) => 
                        item.StepType != StepData.StepTypes.FreshStart &&
                        item.StepType != StepData.StepTypes.Seperator);

                return effectiveSteps > 0;
            }
        }
        public bool IsChanged { get; set ; }
        public void Save()
        {
            AppSettings.Data.StepDataList = StepList.ToArray();
            AppSettings.Save();
        }

        //Props
        #region Dependency Properties
        public static readonly DependencyProperty StepListProperty =
            DependencyProperty.Register("StepList", typeof(ObservableCollection<StepData>), typeof(StepsEditorSettingPage), new PropertyMetadata());
        public static readonly DependencyProperty SelectedStepProperty =
            DependencyProperty.Register("SelectedStep", typeof(StepData), typeof(StepsEditorSettingPage), new PropertyMetadata());
        #endregion
        public ObservableCollection<StepData> StepList
        {
            get { return (ObservableCollection<StepData>)GetValue(StepListProperty); }
            set { SetValue(StepListProperty, value); }
        }
        public StepData SelectedStep
        {
            get { return (StepData)GetValue(SelectedStepProperty); }
            set { SetValue(SelectedStepProperty, value); }
        }
        public bool IsSelected => SelectedStep != null;

        //Arrays
        public Array StepTypesArray { get; set; }
        public Array ImageListesArray { get; set; }

        //ctor
        public StepsEditorSettingPage()
        {
            StepTypesArray = Enum.GetValues(typeof(StepData.StepTypes));
            ImageListesArray = Enum.GetValues(typeof(StepData.ImageListes));

            SetCloneOfStepDataArray(AppSettings.Data.StepDataList);

            StepList.CollectionChanged += StepList_CollectionChanged;
            
            DataContext = this;
            InitializeComponent();
        }

        private void SetCloneOfStepDataArray(StepData[] stepDataList)
        {
            StepList = new ObservableCollection<StepData>(stepDataList.Select((i) =>(StepData)i.Clone()));
        }





        //Templates
        bool isChangedFromLastTemplateChange = true;
        private void cboTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTemplates.SelectedItem == null)
                return;

            object tag = ((ComboBoxItem)cboTemplates.SelectedItem).Tag;

            if (tag == null)
                return;

            StepData[] template = (StepData[])App.Current.TryFindResource(tag);
            if (template != null)
            {
                if (isChangedFromLastTemplateChange)
                {
                    if(!YesNoMessageBox("Changes","Are You Sure Clear Changes?"))
                    {
                        cboTemplates.SelectedItem = e.RemovedItems[0];
                        return;
                    }
                }

                SetCloneOfStepDataArray(template);
                IsChanged = true;
                isChangedFromLastTemplateChange = false;
            }
        }
        private static bool YesNoMessageBox(string caption,string message)
        {
            return AdonisUI.Controls.MessageBoxResult.Yes == AdonisUI.Controls.MessageBox.Show(message, caption, AdonisUI.Controls.MessageBoxButton.YesNo);
        }

        //Changes Detection
        private void StepList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsChanged = true;
            isChangedFromLastTemplateChange = true;
        }

        private void OnPropertiesChanged(object sender, DataTransferEventArgs e) => UserChangeValues();
        private void lstItems_Drop(object sender, DragEventArgs e) => UserChangeValues();
        private void UserChangeValues()
        {
            cboTemplates.SelectedIndex = 2;
            IsChanged = true;
            isChangedFromLastTemplateChange = true;
        }


        //commands
        #region Commands
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StepData step = new StepData();
            if (IsSelected)
            {
                int index = lstItems.SelectedIndex + 1;
                StepList.Insert(index, step);
                SelectedStep = step;
            }
            else
                StepList.Add(step);
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsSelected;
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int selIndex = lstItems.SelectedIndex;
            StepList.RemoveAt(selIndex);
            lstItems.SelectedIndex = Math.Min(selIndex, lstItems.Items.Count - 1);
            lstItems.Focus();
        }
        #endregion

    }
}
