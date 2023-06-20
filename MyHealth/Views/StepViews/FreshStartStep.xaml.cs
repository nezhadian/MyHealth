using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
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
    /// Interaction logic for FreshStart.xaml
    /// </summary>
    public partial class FreshStartStep : Page
    {
        public FreshStartStep()
        {
            DataContext = AppSettings.Data;
            InitializeComponent();
        }

        static SoundPlayer sp;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAndPlaySoundAsync();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StopSoundAsync();
        }

        private void LoadAndPlaySoundAsync()
        {
            Task.Run(delegate
            {
                try
                {
                    if (sp == null)
                    {
                        sp = new SoundPlayer(App.GetAssetsPath("Songs", "alarm.wav"));
                        sp.Load();
                    }
                    sp.PlayLooping();

                }
                catch (Exception ex)
                {
                    Utils.ErrorMessageBox(ex);
                }


            });
        }
        private void StopSoundAsync()
        {
            Task.Run(delegate
            {
                sp?.Stop();
            });
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            StopSoundAsync();
            btnMute.Visibility = Visibility.Collapsed;
        }
    }
}
