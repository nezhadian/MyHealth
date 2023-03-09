using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyHealth
{
    public class Timer : Control
    {
        #region DependencyProperties
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(Timer), new PropertyMetadata());
        public static readonly DependencyProperty ElapsedTimeProperty =
            DependencyProperty.Register("ElapsedTime", typeof(TimeSpan), typeof(Timer), new PropertyMetadata());
        public static readonly DependencyProperty RemainedTimeProperty =
            DependencyProperty.Register("RemainedTime", typeof(TimeSpan), typeof(Timer), new PropertyMetadata());
        public static readonly DependencyProperty IsPausedProperty =
            DependencyProperty.Register("IsPaused", typeof(bool), typeof(Timer), new PropertyMetadata());
        #endregion

        DispatcherTimer updater = new DispatcherTimer();
        DateTime LastStartTime;
        TimeSpan? StopedTime;

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { 
                SetValue(DurationProperty, value);
                StopedTime = TimeSpan.Zero;
                ElapsedTime = TimeSpan.Zero;
                RemainedTime = value;
                IsPaused = value == TimeSpan.Zero;
            }
        }
        public TimeSpan ElapsedTime
        {
            get { return (TimeSpan)GetValue(ElapsedTimeProperty); }
            set { SetValue(ElapsedTimeProperty, value); }
        }
        public TimeSpan RemainedTime
        {
            get { return (TimeSpan)GetValue(RemainedTimeProperty); }
            set { SetValue(RemainedTimeProperty, value); }
        }
        
        public bool IsPaused
        {
            get { return (bool)GetValue(IsPausedProperty); }
            set { 
                SetValue(IsPausedProperty, value);
                updater.IsEnabled = !value;
                if (value)
                    StopedTime = ElapsedTime;
                else
                    LastStartTime = DateTime.Now - StopedTime.Value;
            }
        }


        public event RoutedEventHandler Completed;

        public Timer()
        {
            updater = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            updater.Tick += Updater_Tick;

            SetBinding(IsPausedProperty, new Binding("IsEnabled")
            {
                Source = this,
                Mode = BindingMode.OneWayToSource
            });
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            ElapsedTime = DateTime.Now - LastStartTime;
            RemainedTime = Duration - ElapsedTime;

            if (RemainedTime <= TimeSpan.Zero)
                Completed?.Invoke(this, null);
        }


    }
}
