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
        public static readonly DependencyProperty InProgressProperty =
            DependencyProperty.Register("InProgress", typeof(bool), typeof(TaskView), new PropertyMetadata(false));
        public static readonly DependencyProperty IsCompletedProperty =
            DependencyProperty.Register("IsCompleted", typeof(bool), typeof(TaskView), new PropertyMetadata(false));
        public static readonly DependencyProperty IsImportantProperty =
            DependencyProperty.Register("IsImportant", typeof(bool), typeof(TaskView), new PropertyMetadata(false));
        #endregion

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool InProgress
        {
            get { return (bool)GetValue(InProgressProperty); }
            set { SetValue(InProgressProperty, value); }
        }

        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, value); }
        }

        public bool IsImportant
        {
            get { return (bool)GetValue(IsImportantProperty); }
            set { SetValue(IsImportantProperty, value); }
        }

    }
}
