using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyHealth
{
    public class StepListViewModel : StepListViewModelBase
    {

        private StepData[] _stepsArray;
        public StepData[] StepsArray
        {
            get => _stepsArray;
            set
            {
                _stepsArray = value;
                OnPropertyChanged();
                SelectedStepIndex = 0;
            }
        }

        public StepListIgnoreSeperatorsCommand IgnoreSeperatorsCommand { set; get; }
        public StepListClickCommand ClickCommand { set; get; }

        public StepListViewModel()
        {
            StepsArray = new StepData[0];
            IgnoreSeperatorsCommand = new StepListIgnoreSeperatorsCommand(this);
            ClickCommand = new StepListClickCommand(this);
        }
        public void GoToNextStep()
        {
            if (StepsArray.Length == 0)
                return;

            int nextIndex = SelectedStepIndex;
            nextIndex++;
            if (nextIndex >= StepsArray.Length)
                nextIndex = 0;
            SelectedStepIndex = nextIndex;
        }
    }

    public class StepListIgnoreSeperatorsCommand : ContextCommand<StepListViewModel>
    {
        public StepListIgnoreSeperatorsCommand(StepListViewModel context) : base(context) { }

        public override bool CanExecute(StepListViewModel context, object parameter)
        {
            return context.SelectedStep != null && context.SelectedStep.StepType == StepData.StepTypes.Seperator;
        }
        public override void Execute(StepListViewModel context, object parameter)
        {
            context.GoToNextStep();
        }
    }
    public class StepListClickCommand : ContextCommand<StepListViewModel>
    {
        public StepListClickCommand(StepListViewModel context) : base(context) { }

        public override bool CanExecute(StepListViewModel context, object parameter)
        {
            return context.SelectedStep.StepType == StepData.StepTypes.FreshStart;
        }
        public override void Execute(StepListViewModel context, object parameter)
        {
            context.GoToNextStep();
        }
    }

}
