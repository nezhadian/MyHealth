using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MyHealth
{
    public class TaskView : DependencyObject
    {
        #region DependencyProperties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TaskView), new PropertyMetadata());
        public static readonly DependencyProperty GroupingProperty =
                    DependencyProperty.Register("Grouping", typeof(TaskGroups), typeof(TaskView), new PropertyMetadata(TaskGroups.Tasks));
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

        

    }
}
