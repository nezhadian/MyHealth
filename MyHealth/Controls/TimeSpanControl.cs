using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public class TimeSpanControl : Control
    {
        #region Dp
        public static readonly DependencyProperty TimeSpanProperty =
            DependencyProperty.Register("TimeSpan", typeof(TimeSpan), typeof(TimeSpanControl), new PropertyMetadata(TimeSpan.Zero,OnTimeSpanChanged));
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimeSpanControl), new PropertyMetadata(0, OnMinuteChanged));
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(int), typeof(TimeSpanControl), new PropertyMetadata(0, OnSecondsChanged));
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimeSpanControl), new PropertyMetadata(0, OnHoursChanged));


        public static readonly DependencyProperty HourVisibilityProperty =
            DependencyProperty.Register("HourVisibility", typeof(Visibility), typeof(TimeSpanControl), new PropertyMetadata(Visibility.Visible));
        #endregion
        public TimeSpan TimeSpan
        {
            get { return (TimeSpan)GetValue(TimeSpanProperty); }
            set { SetValue(TimeSpanProperty, value); }
        }

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }
        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public Visibility HourVisibility
        {
            get { return (Visibility)GetValue(HourVisibilityProperty); }
            set { SetValue(HourVisibilityProperty, value); }
        }

        //Static Dependency Properties Change Callbacks
        private static void OnTimeSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl tsc = (TimeSpanControl)d;
            TimeSpan timeSpan = (TimeSpan)e.NewValue;
            tsc.Hours = timeSpan.Hours;
            tsc.Minutes = timeSpan.Minutes;
            tsc.Seconds = timeSpan.Seconds;
        }

        private static void OnMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl tsc = (TimeSpanControl)d;
            int minutes = (int)e.NewValue;
            tsc.TimeSpan = new TimeSpan(tsc.Hours, minutes, tsc.Seconds);
        }
        private static void OnSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl tsc = (TimeSpanControl)d;
            int seconds = (int)e.NewValue;
            tsc.TimeSpan = new TimeSpan(tsc.Hours, tsc.Minutes, seconds);
        }
        private static void OnHoursChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl tsc = (TimeSpanControl)d;
            int hours = (int)e.NewValue;
            tsc.TimeSpan = new TimeSpan(hours, tsc.Minutes, tsc.Seconds);
        }
    }
}
