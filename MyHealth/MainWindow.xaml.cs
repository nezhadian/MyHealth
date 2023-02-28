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
                mnuContinue.IsEnabled = value;
                mnuStop.IsEnabled = !value;

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
                    frmMain.Content = CurrentPage;
                    StopedTime = new TimeSpan(0);
                    IsPaused = CurrentPage.RequireClick;
                    txtTimer.Text = "More";
                }
                else
                {
                    IsPaused = true;
                }
            }
        }

        public ITimerSlice CurrentPage => CurrentIndex < Steps.Length ? Steps[CurrentIndex] : null;
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

        private void ExitBtn_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);
        private void ZeroTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            StopedTime = new TimeSpan(0);
            IsPaused = false;
        }

        private void mnuStop_Click(object sender, RoutedEventArgs e) => IsPaused = true;
        private void mnuContinue_Click(object sender, RoutedEventArgs e) => IsPaused = false;

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            lstSteps.Items.Clear();

            int i = 0;

            foreach (StepData item in DataAccess.StepDataList)
            {
                switch (item.StepType)
                {
                    case StepData.StepTypes.Seperator:
                        lstSteps.Items.Add(new Separator());
                        break;
                    default:
                        MenuItem menuItem = new MenuItem()
                        {
                            Header = item.ToString(),
                            Tag = i,
                            IsCheckable = true,
                            IsChecked = i == CurrentIndex
                        };
                        menuItem.Click += MenuItem_Click;
                        lstSteps.Items.Add(menuItem);
                        i++;
                        break;
                }

                
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex = (int)(sender as MenuItem).Tag;
        }

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
    }

    public interface ITimerSlice 
    {
        public TimeSpan Duration { set; get; }
        public bool RequireClick { set; get; }
        public string StepName { set; get; }
    }
}
