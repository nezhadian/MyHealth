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
    /// Interaction logic for FreshStart.xaml
    /// </summary>
    public partial class FreshStart : Page,ITimerSlice
    {
        public TimeSpan Duration { get; set; }
        public bool RequireClick { get; set; }

        public FreshStart()
        {
            InitializeComponent();
            RequireClick = true;
        }

        DispatcherTimer timer;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += (s,eb) => Console.Beep(1000, 1000);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
