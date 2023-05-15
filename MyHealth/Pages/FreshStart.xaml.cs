using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
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
    public partial class FreshStart : Page
    {
        public TimeSpan Duration { get; set; }
        public bool RequireClick { get; set; }
        public string StepName {
            get => txtMessage.Text;
            set => txtMessage.Text = value;
        } 

        public FreshStart()
        {
            InitializeComponent();
            RequireClick = true;
        }

        public FreshStart(StepData step)
        {
        }

        SoundPlayer sp;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                sp = new SoundPlayer($"{Environment.CurrentDirectory}\\Songs\\alarm.wav");
                sp.Load();
                sp.PlayLooping();
                btnMute.Visibility = Visibility.Visible;
            }catch { }
            Background = new SolidColorBrush(AppSettings.Data.FreshStartBgColor);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) => sp?.Stop();
        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            sp?.Stop();
            btnMute.Visibility = Visibility.Collapsed;
        }
    }
}
