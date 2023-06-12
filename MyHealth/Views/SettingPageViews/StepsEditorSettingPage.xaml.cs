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

        StepsEditorViewModel StepsEditorViewModel;

        //ISavebleSettingItem Implementation
        public bool IsChanged { get; set ; }
        public void SetValuesToAppSettings()
        {
            AppSettings.Data.StepDataList = StepsEditorViewModel.StepList.ToArray();
        }
        public bool CanSave()
        {
            int effectiveSteps = StepsEditorViewModel.StepList.Count((item) =>
                item.StepType != StepData.StepTypes.FreshStart &&
                item.StepType != StepData.StepTypes.Seperator);

            bool canSave = effectiveSteps > 0;

            if (!canSave)
            {
                Utils.InfoMessageBox("Steps Editor","there is no effective Steps ");
            }

            return canSave;
        }
        public void UndoChanges() { }
        public void Reset()
        {
            StepsEditorViewModel.SetStepListFromArray(AppSettings.Data.StepDataList);
            IsChanged = false;

        }

        //fields
        bool isChangedFromLastTemplateChange = true;
        bool isChangingTemplate = false;

        //ctor
        public StepsEditorSettingPage()
        {
            StepsEditorViewModel = new StepsEditorViewModel();

            DataContext = StepsEditorViewModel;
            InitializeComponent();

            StepsEditorViewModel.StepList.CollectionChanged += StepList_CollectionChanged;
        }

        //Templates
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
                    if(!Utils.YesNoMessageBox("Changes","do you want to Clear Changes?","Clear All","Don`t Clear"))
                    {
                        cboTemplates.SelectedItem = e.RemovedItems[0];
                        return;
                    }
                }
                isChangingTemplate = true;

                StepsEditorViewModel.SetStepListFromArray(template);

                isChangingTemplate = false;
                IsChanged = true;
                isChangedFromLastTemplateChange = false;
            }
        }

        //Changes Detection
        private void OnPropertiesChanged(object sender, DataTransferEventArgs e) => UserChangeValues();
        private void StepList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UserChangeValues();
        }
        private void UserChangeValues()
        {
            if(!isChangingTemplate)
                cboTemplates.SelectedIndex = 2;

            IsChanged = true;
            isChangedFromLastTemplateChange = true;
        }

        //Keep Focus When Item Deleted
        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).Focus();
        }
    }
}
