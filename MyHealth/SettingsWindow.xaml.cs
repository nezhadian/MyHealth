﻿using System;
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
    public partial class SettingsWindow : Window
    {

        public ObservableCollection<StepData> StepList = new ObservableCollection<StepData>();

        public bool IsSelected => lstItems.SelectedIndex != -1;

        public StepData SelectedStep => StepList[lstItems.SelectedIndex];

        public SettingsWindow()
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
        #endregion
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
            if(tag != "")
            {

            }
            if(Templates.TemplateDictionary.TryGetValue(tag,out StepData[] template))
            {
                AddRange(template);
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

    public class Templates
    {

        public static Dictionary<string, StepData[]> TemplateDictionary = new Dictionary<string, StepData[]>()
        {
            {"pomo" , new StepData[]{
                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "5min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,5,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "5min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,5,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "5min Short Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Eye,Duration = new TimeSpan(0,5,0)},
                        new StepData(){StepName = "Ready", StepType = StepData.StepTypes.FreshStart},

                        new StepData(){StepType = StepData.StepTypes.Seperator},

                        new StepData(){StepName = "25min Work", StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,25,0)},
                        new StepData(){StepName = "20min Long Break", StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Body,Duration = new TimeSpan(0,20,0)},
                        new StepData(){StepName = "Ready For New Pomoduro", StepType = StepData.StepTypes.FreshStart},
            }},

            {"standard" , new StepData[]{
                        new StepData(){StepName = "52min Work",StepType = StepData.StepTypes.WorkTime,Duration = new TimeSpan(0,52,0)},
                        new StepData(){StepName = "27min Resting",StepType = StepData.StepTypes.ImageSlider,ImageList =  StepData.ImageListes.Body,Duration = new TimeSpan(0,17,0)},
                        new StepData(){StepName = "Ready",StepType = StepData.StepTypes.FreshStart},
                        }},
        };


    }

    public class StepData
    {
        public enum StepTypes
        {
            WorkTime,
            ImageSlider,
            FreshStart,
            Seperator
        }
        public StepTypes StepType;

        public string StepName;

        public TimeSpan Duration;

        public enum ImageListes
        {
            Body,Eye
        }
        public ImageListes ImageList;

        public override string ToString()
        {
            switch (StepType)
            {
                case StepTypes.WorkTime:
                    return $"{StepName} | {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.ImageSlider:
                    return $"{StepName} | ({ImageList}) {Duration.ToString("hh':'mm':'ss")}";
                case StepTypes.FreshStart:
                    return $"{StepName}";
                default:
                    return "";
            }
        }
    }
}
