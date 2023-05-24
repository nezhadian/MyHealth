using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace MyHealth
{
    public class TaskView : DependencyObject,INotifyPropertyChanged
    {

        #region DependencyProperties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TaskView), new PropertyMetadata("",OnTitleChanged));

        public static readonly DependencyProperty GroupingProperty =
                    DependencyProperty.Register("Grouping", typeof(TaskGroups), typeof(TaskView), new PropertyMetadata(TaskGroups.Tasks,OnGroupingChanged));

        

        #endregion

        public enum TaskGroups
        {
            Importants, Tasks, Completed
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public TaskGroups Grouping
        {
            get { return (TaskGroups)GetValue(GroupingProperty); }
            set { SetValue(GroupingProperty, value); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskView taskView = (TaskView)d;
            taskView.PropertyChanged?.Invoke(taskView, new PropertyChangedEventArgs("Title"));
        }
        private static void OnGroupingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskView taskView = (TaskView)d;
            taskView.PropertyChanged?.Invoke(taskView, new PropertyChangedEventArgs("Grouping"));
        }

    }
}
