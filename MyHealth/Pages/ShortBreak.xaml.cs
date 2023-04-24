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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for ShortBreak.xaml
    /// </summary>
    public partial class ShortBreak : Page,ITimerSlice
    {
        public TimeSpan Duration { get ; set ; }
        public bool RequireClick { get; set; }
        public string StepName
        {
            set => txtMessage.Text = value;
            get => txtMessage.Text;
        }

        public ShortBreak()
        {
            InitializeComponent();
        }
        public ShortBreak(TimeSpan duration)
        {
            InitializeComponent();
            Duration = duration;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Background = new SolidColorBrush(AppSettings.Default.ShortBreakBgColor);
        }
    }
}
