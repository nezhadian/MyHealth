﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyHealth
{
    public class StepsEditorViewModel : StepListViewModelBase
    {
        public MyObservableCollection<StepData> StepList { get; set; }

        public override int SelectedStepIndex
        {
            get => base.SelectedStepIndex;
            set
            {
                base.SelectedStepIndex = value;
                DeleteCommand.RaiseCanExecuteChanged();
                NewCommand.RaiseCanExecuteChanged();
            }
        }

        public StepsEditorNewCommand NewCommand { get; set; }
        public StepsEditorDeleteCommand DeleteCommand { get; set; }

        public StepsEditorViewModel()
        {
            StepList = new MyObservableCollection<StepData>();

            NewCommand = new StepsEditorNewCommand(this);
            DeleteCommand = new StepsEditorDeleteCommand(this);
        }
        internal void SetStepListFromArray(StepData[] collection)
        {
            StepList.Clear();
            StepList.AddRange(collection);

            SelectedStepIndex = 0;
        }
    }
    public class StepsEditorNewCommand : ContextCommand<StepsEditorViewModel>
    {
        public StepsEditorNewCommand(StepsEditorViewModel context) : base(context) { }

        public override bool CanExecute(StepsEditorViewModel context, object parameter)
        {
            return context.StepList.Count < 100;
        }

        public override void Execute(StepsEditorViewModel context, object parameter)
        {
            StepData step = new StepData();
            if (context.SelectedStepIndex != -1)
            {
                int index = context.SelectedStepIndex + 1;
                context.StepList.Insert(index, step);
            }
            else
            {
                context.StepList.Add(step);
            }

            context.SelectedStep = step;
        }
    }
    public class StepsEditorDeleteCommand : ContextCommand<StepsEditorViewModel>
    {
        public StepsEditorDeleteCommand(StepsEditorViewModel context) : base(context) { }

        public override bool CanExecute(StepsEditorViewModel context, object parameter)
        {
            return context.SelectedStepIndex != -1;
        }

        public override void Execute(StepsEditorViewModel context, object parameter)
        {

            int selIndex = context.SelectedStepIndex;
            context.StepList.RemoveAt(selIndex);
            context.SelectedStepIndex = Math.Min(selIndex, context.StepList.Count - 1);
        }
    }
}
