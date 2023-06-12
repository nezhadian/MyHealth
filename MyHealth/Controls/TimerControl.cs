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
    /// <summary>
    /// Interaction logic for TimerControl.xaml
    /// </summary>
    public partial class TimerControl : Control
    {
        #region Dp
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(TimerControl), new PropertyMetadata(new TimeSpan(0, 0, 10), OnDurationChanged));
        public static readonly DependencyProperty IsPausedProperty =
            DependencyProperty.Register("IsPaused", typeof(bool), typeof(TimerControl), new PropertyMetadata(false, OnIsPausedChanged));

        public static readonly DependencyProperty RemainedProperty =
            DependencyProperty.Register("Remained", typeof(TimeSpan), typeof(TimerControl), new PropertyMetadata(TimeSpan.Zero));
        public static readonly DependencyProperty ElapsedProperty =
            DependencyProperty.Register("Elapsed", typeof(TimeSpan), typeof(TimerControl), new PropertyMetadata(TimeSpan.Zero));
        #endregion
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
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

        #region Private Fields
        DispatcherTimer timerBase;
        DateTime offsetTime;
        #endregion

        #region Events
        public event RoutedEventHandler Completed;
        #endregion

        #region Commands
        public TimerControlResetCommand ResetCommand { get; set; }
        #endregion

        #region ctor
        public TimerControl()
        {
            InitializeTimer();
            ResetCommand = new TimerControlResetCommand(this);
        }
        #endregion

        #region private Methods
        void InitializeTimer()
        {
            timerBase = new DispatcherTimer()
            {
                Interval = new TimeSpan(0,0,0,0,900),
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
        #endregion

        #region Public Methods
        public void Reset()
        {
            offsetTime = DateTime.Now;
            Elapsed = TimeSpan.Zero;
            Remained = TimeSpan.Zero;
            IsPaused = Duration == TimeSpan.Zero;
        }
        #endregion

        #region Static Property Change Methods
        private static void OnIsPausedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimerControl timer = (TimerControl)d;
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
            TimerControl timer = (TimerControl)d;
            timer.Reset();
        }
        #endregion

    }
    public class TimerControlResetCommand : ContextCommand<TimerControl>
    {
        public TimerControlResetCommand(TimerControl context) : base(context) { }

        public override bool CanExecute(TimerControl context, object parameter)
        {
            return context.Duration != TimeSpan.Zero;
        }

        public override void Execute(TimerControl context, object parameter)
        {
            context.Reset();
        }
    }
}
