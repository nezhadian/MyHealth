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
        public string StepName { get; set; } = "Image Slider";

        public FileInfo[] imageFiles;
        DispatcherTimer timer;

        int curIndex;
        public int CurrentIndex
        {
            get => curIndex;
            set
            {
                curIndex = value >= imageFiles.Length ? 0 : value;

                if (curIndex < imageFiles.Length)
                {
                    try
                    {
                        mainImage.Source = new BitmapImage(new Uri(imageFiles[value].FullName));
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Error in loading image \r\n {ex.Message}");
                    }
                    timer?.Start();
                }
                else
                {
                    timer?.Stop();
                }
            }
        }

        private DirectoryInfo imageFilesDir;
        public DirectoryInfo ImagesFileDirectory
        {
            get => imageFilesDir;
            set
            {
                if (!value.Exists)
                    return;

                var files = value.GetFiles();
                if (files.Length == 0)
                    return;

                imageFilesDir = value;
                imageFiles = files;
                CurrentIndex = 0;
            }
        }


        public ImageSlider()
        {
            InitializeComponent();
        }
        public ImageSlider(TimeSpan duration,string path)
        {
            InitializeComponent();
            Duration = duration;
            ImagesFileDirectory = new DirectoryInfo(path);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 20),//Image Changing Time
                IsEnabled = true
            };
            timer.Tick += (s,ev) => CurrentIndex++;
        }
    }
}
