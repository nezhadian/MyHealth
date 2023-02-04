using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ShortBreak.xaml
    /// </summary>
    public partial class ImageSlider : Page,ITimerSlice
    {
        public TimeSpan Duration { get; set; }
        public bool RequireClick { get; set; }
        public string StepName { get; set; } = "Short Break";

        private const string IMAGES_FOLDER = "images";
        static string[] imageFiles;
        static int index = 0;

        public ImageSlider()
        {
            InitializeComponent();
        }

        public ImageSlider(int minutes)
        {
            Duration = new TimeSpan(0, minutes, 0);
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(imageFiles == null && Directory.Exists(IMAGES_FOLDER))
                imageFiles = Directory.GetFiles(IMAGES_FOLDER);

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 20),//Image Changing Time
                IsEnabled = true
            };
            timer.Tick += SlideImages;
        }

        private void SlideImages(object sender, EventArgs e)
        {
            if(index < imageFiles.Length)
            {
                try
                {
                    imgHint.Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\{imageFiles[index]}"));
                }
                catch
                {
                    MessageBox.Show("Error in loading images");
                }
            }

            index++;
            if (index >= imageFiles.Length)
                index = 0;
        }
    }
}
