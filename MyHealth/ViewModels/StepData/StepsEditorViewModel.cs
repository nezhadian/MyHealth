using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyHealth
{
    public class StepsEditorViewModel : StepListViewModelBase
    {
        public ObservableCollection<StepData> StepList { get; set; }

        public StepsEditorNewCommand NewCommand { get; set; }
        public StepsEditorDeleteCommand DeleteCommand { get; set; }

        public StepsEditorViewModel()
        {
            StepList = new System.Collections.ObjectModel.ObservableCollection<StepData>();

            NewCommand = new StepsEditorNewCommand(this);
            DeleteCommand = new StepsEditorDeleteCommand(this);
        }
        internal void SetStepListFromArray(StepData[] collection)
        {
            StepList.Clear();

            foreach (var item in collection)
            {
                StepList.Add((StepData)item.Clone());
            }

            SelectedStepIndex = 0;
        }
    }
    public class StepsEditorNewCommand : ContextCommand<StepsEditorViewModel>
    {
        public StepsEditorNewCommand(StepsEditorViewModel context) : base(context) { }

        public override bool CanExecute(StepsEditorViewModel context, object parameter)
        {
            return true;
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
            return true;
        }

        public override void Execute(StepsEditorViewModel context, object parameter)
        {
            if (context.SelectedStepIndex == -1)
                return;

            int selIndex = context.SelectedStepIndex;
            context.StepList.RemoveAt(selIndex);
            context.SelectedStepIndex = Math.Min(selIndex, context.StepList.Count - 1);
        }
    }
}
