using System;
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
    public class StepData : DependencyObject,ICloneable
    {
        #region Dependency Properites
        public static readonly DependencyProperty StepTypeProperty =
            DependencyProperty.Register("StepType", typeof(StepTypes), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty StepNameProperty =
            DependencyProperty.Register("StepName", typeof(string), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty ImageListProperty =
            DependencyProperty.Register("ImageList", typeof(ImageListes), typeof(StepData), new PropertyMetadata());
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(StepData), new PropertyMetadata());

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
            get { return (StepTypes)GetValue(StepTypeProperty); }
            set { SetValue(StepTypeProperty, value); }
        }
        public string StepName
        {
            get { return (string)GetValue(StepNameProperty); }
            set { SetValue(StepNameProperty, value); }
        }
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public ImageListes ImageList
        {
            get { return (ImageListes)GetValue(ImageListProperty); }
            set { SetValue(ImageListProperty, value); }
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
