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

        public ITimerSlice[] Steps = new ITimerSlice[]
        {
            new FreshStart(),
            new WorkTime(52),
            new ShortBreak(17),
        };

        DispatcherTimer updater = new DispatcherTimer();
        DateTime LastStartTime;

        int curIndex;
        public int CurrentIndex
        {
            get => curIndex;
            set
            {
                if (value >= Steps.Length)
                    curIndex = 0;
                else
                    curIndex = value;

                if(curIndex < Steps.Length)
                {
                    frmExrecise.Content = CurrentPage;
                    LastStartTime = DateTime.Now;
                    updater.IsEnabled = !CurrentPage.RequireClick;
                }
                else
                {
                    updater.IsEnabled = false;
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
            updater = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            updater.Tick += Updater_Tick;
            CurrentIndex = 0;
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            if (Remained < TimeSpan.Zero)
                CurrentIndex++;

            txtTimer.Text = Remained.ToString("mm':'ss");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentPage.RequireClick)
            {
                CurrentIndex++;
            }
        }

    }

    public interface ITimerSlice 
    {
        public TimeSpan Duration { set; get; }
        public bool RequireClick { set; get; }

        
    }
    public class WorkTime : ITimerSlice
    {

        public TimeSpan Duration { get; set; }
        public bool RequireClick { get; set; }

        public WorkTime(int minutes)
        {
            Duration = new TimeSpan(0, minutes, 0);
        }
        public override string ToString()
        {
            return "";
        }
    }
}
