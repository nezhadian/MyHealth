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
            }
        }

        public StepListIgnoreSeperatorsCommand IgnoreSeperatorsCommand { set; get; }
        public StepListClickCommand ClickCommand { set; get; }

        public StepListViewModel()
        {
            IgnoreSeperatorsCommand = new StepListIgnoreSeperatorsCommand(this);
            ClickCommand = new StepListClickCommand(this);

            AppSettings.Data.PropertyChanged += Data_PropertyChanged;
        }

        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppSettings.Data.StepDataList))
            {
                ReloadStepsFromSettings();
            }
        }

        public void ReloadStepsFromSettings()
        {
            StepsArray = AppSettings.Data.StepDataList;
            SelectedStepIndex = 0;
        }
        public void GoToNextStep()
        {
            int curIndex = SelectedStepIndex;
            curIndex++;
            curIndex %= StepsArray.Length;
            SelectedStepIndex = curIndex;
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
