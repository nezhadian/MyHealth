using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Timer : DependencyObject
    {
        #region Dp
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(Timer), new PropertyMetadata(new TimeSpan(0,0,10),OnDurationChanged));
        public static readonly DependencyProperty IsPausedProperty =
            DependencyProperty.Register("IsPaused", typeof(bool), typeof(Timer), new PropertyMetadata(false, OnIsPausedChanged));

        public static readonly DependencyProperty RemainedProperty =
            DependencyProperty.Register("Remained", typeof(TimeSpan), typeof(Timer), new PropertyMetadata(TimeSpan.Zero));
        public static readonly DependencyProperty ElapsedProperty =
            DependencyProperty.Register("Elapsed", typeof(TimeSpan), typeof(Timer), new PropertyMetadata(TimeSpan.Zero));
        #endregion
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value);}
        }
        public bool IsPaused
        {
            get { return (bool)GetValue(IsPausedProperty); }
            set { SetValue(IsPausedProperty, value); }
        }
        public TimeSpan Remained
        {
            get { return (TimeSpan)GetValue(RemainedProperty); }
            set { SetValue(RemainedProperty, value); }
        }
        public TimeSpan Elapsed
        {
            get { return (TimeSpan)GetValue(ElapsedProperty); }
            set { SetValue(ElapsedProperty, value); }
        }

        //Private Fields
        DispatcherTimer timerBase;
        DateTime offsetTime;

        //Events
        public event RoutedEventHandler Completed;

        //ctor
        public Timer()
        {
            InitializeTimer();
        }

        //private Methods
        void InitializeTimer()
        {
            timerBase = new DispatcherTimer()
            {
                Interval = new TimeSpan(10000),
                IsEnabled = true
            };
            timerBase.Tick += Updater_Tick;
        }
        void Updater_Tick(object sender, EventArgs e)
        {
            if (IsPaused)
                return;

            Elapsed = DateTime.Now - offsetTime;
            Remained = Duration - Elapsed;
            if (Remained <= TimeSpan.Zero)
            {
                Completed?.Invoke(this, null);
            }
        }

        //Public Methods
        public void Reset()
        {
            offsetTime = DateTime.Now;
            Elapsed = TimeSpan.Zero;
            Remained = TimeSpan.Zero;
            IsPaused = Duration == TimeSpan.Zero;
        }

        //Static Property Change Methods
        private static void OnIsPausedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Timer timer = (Timer)d;
            if (timer.IsPaused)
            {
                timer.timerBase.IsEnabled = false;
            }
            else
            {
                timer.timerBase.IsEnabled = true;
                timer.offsetTime = DateTime.Now - timer.Elapsed;
            }
        }
        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Timer timer = (Timer)d;
            timer.Reset();
        }
    }
}
