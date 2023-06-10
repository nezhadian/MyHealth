using System.Collections.ObjectModel;

namespace MyHealth
{
    public class StepListViewModelBase : ViewModelBase
    {

        private StepData _selStep;
        public StepData SelectedStep
        {
            get => _selStep;
            set
            {
                _selStep = value;
                OnPropertyChanged();
            }
        }

        private int _selStepIndex;
        public int SelectedStepIndex
        {
            get => _selStepIndex;
            set
            {
                _selStepIndex = value;
                OnPropertyChanged();
            }
        }
    }
}