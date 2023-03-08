using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ITimerSlice[] Steps;

        DispatcherTimer updater = new DispatcherTimer();
        DateTime LastStartTime;
        TimeSpan? StopedTime;

        bool isPaused;
        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                updater.IsEnabled = !value;
                mnuPlayPause.IsChecked = value;

                if (value)
                    StopedTime = Elapsed;
                else
                    LastStartTime = DateTime.Now - StopedTime.Value;
            }
        }

        int curIndex;
        public int CurrentIndex
        {
            get => curIndex;
            set
            {
                curIndex = value >= Steps.Length ? 0 : value;

                if (curIndex < Steps.Length)
                {
                    ITimerSlice page = CurrentPage;
                    frmMain.Content = page;
                    StopedTime = new TimeSpan(0);
                    IsPaused = page.RequireClick;
                    txtTimer.Text = "More";
                }
                else
                {
                    IsPaused = true;
                }
            }
        }

        public ITimerSlice CurrentPage => Steps?[CurrentIndex];
        public TimeSpan Elapsed => DateTime.Now - LastStartTime;
        public TimeSpan Remained => CurrentPage.Duration - Elapsed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Steps = DataAccess.GenerateSteps();

            updater = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            updater.Tick += Updater_Tick;
            CurrentIndex = 0;

            BindTopmost();

        }

        private void BindTopmost()
        {
            mnuLockOnScreen.SetBinding(MenuItem.IsCheckedProperty, new Binding("Topmost")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            if (Remained <= TimeSpan.Zero)
                CurrentIndex++;
            else
                txtTimer.Text = Remained.ToString(Remained.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentPage.RequireClick)
            {
                CurrentIndex++;
            }
        }


        #region Context Menu

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            object[] items = DataAccess.GenerateMenuItem();
            int i = 0;
            foreach (var item in items)
            {
                if(item is MenuItem)
                {
                    MenuItem menuItem = (MenuItem)item;
                    menuItem.Tag = i;
                    menuItem.IsChecked = i == CurrentIndex;
                    menuItem.Click += (s,ev) => CurrentIndex = (int)(s as MenuItem).Tag;
                    i++;
                }
            }
            lstSteps.ItemsSource = items;
        }

        private void RestoreWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void RestoreWindow_Executed(object sender, ExecutedRoutedEventArgs e) => Activate();

        private void RestartCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CurrentPage != null ? !CurrentPage.RequireClick : false;
        }
        private void RestartCommand_Executed(object sender, ExecutedRoutedEventArgs e) => CurrentIndex = CurrentIndex;
        private void PausePlayCommand_Executed(object sender, ExecutedRoutedEventArgs e) => IsPaused = !IsPaused;

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWin = new StepEditor();
            if(true == settingsWin.ShowDialog())
            {
                lock (Steps)
                {
                    Steps = DataAccess.GenerateSteps();
                    CurrentIndex = 0;
                }
            }
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);

        #endregion
    }

    public interface ITimerSlice 
    {
        public TimeSpan Duration { set; get; }
        public bool RequireClick { set; get; }
        public string StepName { set; get; }
    }

    public class TimerCommands
    {
        public static RoutedCommand RestartCommand = new RoutedCommand("RestartCommand",typeof(TimerCommands));
        public static RoutedCommand PausePlayCommand = new RoutedCommand("PausePlayCommand", typeof(TimerCommands));
    }
}
