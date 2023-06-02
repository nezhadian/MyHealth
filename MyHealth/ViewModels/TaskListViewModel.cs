using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealth
{
    public class TaskListViewModel : ViewModelBase
    {
        //Private Fields
        private TaskView _selTask;

        //Properties
        public ObservableCollection<TaskView> TaskList { set; get; }
        public TaskView SelectedTask
        {
            get => _selTask;
            set
            {
                _selTask = value;
                OnPropertyChanged();
            }
        }

        public TaskListDropHandler TaskListDropHandler { get; set; }
        public TaskListAddTaskCommand AddTaskCommand { get; set; }
        
        //ctor
        public TaskListViewModel()
        {
            TaskList = new ObservableCollection<TaskView>(AppSettings.Data.TaskList ?? new TaskView[0]);

            Array.ForEach(AppSettings.Data.TaskList, (i) => i.PropertyChanged += OnTaskChanged);
            AddTaskCommand = new TaskListAddTaskCommand(this);
            TaskListDropHandler = new TaskListDropHandler();
        }

        //Public Methods
        public void CreateNewTask(string title)
        {
            TaskView newTask = new TaskView()
            { Title = title };
            newTask.PropertyChanged += OnTaskChanged;

            TaskList.Add(newTask);
            SaveTaskList();
        }

        //Private Methods
        private void OnTaskChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void SaveTaskList()
        {
            //throw new NotImplementedException();
        }
    }

    public class TaskListAddTaskCommand : ViewModelCommands<TaskListViewModel>
    {
        public TaskListAddTaskCommand(TaskListViewModel context) : base(context) { }

        public override bool CanExecute(TaskListViewModel context, object parameter)
        {
            return !string.IsNullOrWhiteSpace(parameter.ToString());
        }
        public override void Execute(TaskListViewModel context, object parameter)
        {
            context.CreateNewTask(parameter.ToString());
        }
    }
}
