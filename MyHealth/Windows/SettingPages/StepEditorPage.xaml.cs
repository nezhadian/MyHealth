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
    public partial class StepEditorPage : Page,ISavebleSettingItem
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
            StepData[] data = new StepData[StepList.Count];
            StepList.CopyTo(data, 0);
            AppSettings.Data.StepDataList = data;
            AppSettings.Save();
        }

        //Props
        #region Dependency Properties
        public static readonly DependencyProperty StepListProperty =
            DependencyProperty.Register("StepList", typeof(ObservableCollection<StepData>), typeof(StepEditorPage), new PropertyMetadata());
        public static readonly DependencyProperty SelectedStepProperty =
            DependencyProperty.Register("SelectedStep", typeof(StepData), typeof(StepEditorPage), new PropertyMetadata());
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
        public StepEditorPage()
        {
            StepList = new ObservableCollection<StepData>(AppSettings.Data.StepDataList);
            StepTypesArray = Enum.GetValues(typeof(StepData.StepTypes));
            ImageListesArray = Enum.GetValues(typeof(StepData.ImageListes));

            StepList.CollectionChanged += StepList_CollectionChanged; ;
            
            DataContext = this;
            InitializeComponent();
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
            lstItems.SelectedIndex = Math.Min(selIndex,lstItems.Items.Count - 1);
            lstItems.Focus();
        }
        #endregion


        //Templates
        private void cboTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 0)
                return;

            bool IsPreviousItemCustom = ((ComboBoxItem)e.RemovedItems[0]).Tag == null;
            bool hasItems = StepList.Count != 0;

            if (IsPreviousItemCustom && hasItems)
            {
                if (YesNoMessageBox("Clear","Clear Changes?"))
                {
                    string tag = ((ComboBoxItem)cboTemplates.SelectedItem).Tag as string;
                    SetTemplateForStepDataList(tag);
                }
                else
                {
                    cboTemplates.SelectedItem = e.RemovedItems[0];
                }
            }
        }
        private void SetTemplateForStepDataList(string key)
        {
            if (Templates.TemplateDictionary.TryGetValue(key, out StepData[] template))
            {
                StepList.Clear();
                AddArrayToList(StepList, template);
            }
        }
        private void AddArrayToList(IList list,Array template)
        {
            foreach (var item in template)
            {
                list.Add(item);
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
        }

        private void UserChangeValues()
        {
            cboTemplates.SelectedIndex = 2;
            IsChanged = true;
        }
        private void UserClickControls(object sender, MouseButtonEventArgs e) => UserChangeValues();
        private void UserPressKeyboardButtons(object sender, KeyEventArgs e) => UserChangeValues();
    }
}
