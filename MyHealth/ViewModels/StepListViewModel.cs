using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyHealth
{
    public class StepListViewModel : StepListViewModelBase
    {
        public StepListIgnoreSeperatorsCommand IgnoreSeperatorsCommand { set; get; }
        public StepListClickCommand ClickCommand { set; get; }

        public StepListViewModel()
        {
            StepList = new ObservableCollection<StepData>(AppSettings.Data.StepDataList);

            IgnoreSeperatorsCommand = new StepListIgnoreSeperatorsCommand(this);
            ClickCommand = new StepListClickCommand(this);

            AppSettings.Data.PropertyChanged += Data_PropertyChanged;
        }

        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppSettings.Data.StepDataList))
                MatchStepList(AppSettings.Data.StepDataList);
        }

        private void MatchStepList(StepData[] list)
        {
            StepList.Clear();
            foreach (var item in list)
            {
                StepList.Add(item);
            }
            SelectedStepIndex = 0;
        }

        internal void GoToNextStep()
        {
            int curIndex = SelectedStepIndex;
            curIndex++;
            curIndex %= StepList.Count;
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
