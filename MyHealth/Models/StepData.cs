using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyHealth
{
    public class StepData : INotifyPropertyChanged,ICloneable
    {
        #region INotifyPropertyChanged Implamentation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Private Fields
        private StepTypes _stepType;
        private string _stepName;
        private TimeSpan _duration;
        private ImageListes _imageList;
        #endregion

        public enum StepTypes
        {
            WorkTime,
            ImageSlider,
            ShortBreak,
            FreshStart,
            Seperator
        }
        public enum ImageListes
        {
            Body,Eye
        }

        public StepTypes StepType
        {
            get { return _stepType; }
            set { _stepType = value; OnPropertyChanged(); }
        }
        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        public ImageListes ImageList
        {
            get => _imageList;
            set
            {
                _imageList = value;
                OnPropertyChanged();
            }
        }

        public object Clone()
        {
            return new StepData()
            {
                StepType = StepType,
                StepName = StepName,
                Duration = Duration,
                ImageList = ImageList
            };
        }

    }
}
