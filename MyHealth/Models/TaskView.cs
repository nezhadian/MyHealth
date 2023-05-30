using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace MyHealth
{
    public class TaskView : INotifyPropertyChanged
    {
        public enum TaskGroups
        {
            Importants, Tasks, Completed
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value;
                OnPropertyChanged();
            }
        }

        private TaskGroups _grouping;
        public TaskGroups Grouping
        {
            get { return _grouping; }
            set { _grouping = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
