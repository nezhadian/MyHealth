﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

                if (SaveCommand.CanExecute(null))
                    SaveCommand.Execute(null);
            }
        }

        public TaskListDropHandler TaskListDropHandler { get; set; }
        public bool NeedSave { get; set; }

        //Commands
        public TaskListAddTaskCommand AddTaskCommand { get; set; }
        public TaskListSaveCommand SaveCommand { get; set; }

        //ctor
        public TaskListViewModel()
        {
            TaskList = new ObservableCollection<TaskView>(AppSettings.Data.TaskList ?? new TaskView[0]);
            Array.ForEach(AppSettings.Data.TaskList, (i) => i.PropertyChanged += OnTaskChanged);

            AddTaskCommand = new TaskListAddTaskCommand(this);
            SaveCommand = new TaskListSaveCommand(this);

            TaskListDropHandler = new TaskListDropHandler();
        }

        //Methods
        public void CreateNewTask(string title)
        {
            TaskView newTask = new TaskView()
            { Title = title };
            newTask.PropertyChanged += OnTaskChanged;

            TaskList.Add(newTask);
            SaveTaskList();
        }

        private void OnTaskChanged(object sender, PropertyChangedEventArgs e)
        {
            NeedSave = true;
        }
        public void SaveTaskList()
        {
            AppSettings.Data.TaskList = TaskList.ToArray();
            AppSettings.Save();
            NeedSave = false;
        }
    }

    public class TaskListAddTaskCommand : ContextCommand<TaskListViewModel>
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
    public class TaskListSaveCommand : ContextCommand<TaskListViewModel>
    {
        public TaskListSaveCommand(TaskListViewModel context) : base(context) { }

        public override bool CanExecute(TaskListViewModel context, object parameter)
        {
            return context.NeedSave;
        }
        public override void Execute(TaskListViewModel context, object parameter)
        {
            context.SaveTaskList();
        }
    }
}