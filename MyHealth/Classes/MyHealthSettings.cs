using System;
using System.ComponentModel;
using System.Windows.Media;

namespace MyHealth
{
    public class MyHealthSettings : SettingData
    {
        //Settings
        [DefaultValue(typeof(Color),"#008000")]
        public Color FreshStartBgColor
        {
            get => (Color)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        [DefaultValue(typeof(Color), "#000000")]
        public Color ShortBreakBgColor
        {
            get => (Color)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        [DefaultValue(typeof(TimeSpan), "00:00:20")]
        public TimeSpan ImageSliderDelay
        {
            get => (TimeSpan)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        [DefaultValue(false)]
        public bool GoOnTaskbar
        {
            get => (bool)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        [DefaultValue(true)]
        public bool StartAtStartup
        {
            get => (bool)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        [DefaultValue("en-us")]
        public string LanguageCode
        {
            get => (string)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        //set and get Dynamic Values
        public override void OnLoaded()
        {
            base.OnLoaded();
            StartAtStartup = App.StartAtStartup;
        }
        public override void OnSaved()
        {
            base.OnSaved();
            App.StartAtStartup = StartAtStartup;
        }

        //Arrays
        private TaskView[] _taskList;
        public TaskView[] TaskList
        {
            get => _taskList ?? new TaskView[0];
            set
            {
                _taskList = value;
                OnPropertyChanged();
            }
        }

        private StepData[] _stepDataList;
        public StepData[] StepDataList
        {
            get => (StepData[])(_stepDataList ?? App.Current.TryFindResource("StepData.PomodoroTemplate"));
            set
            {
                _stepDataList = value;
                OnPropertyChanged();
            }
        }
    }
}
