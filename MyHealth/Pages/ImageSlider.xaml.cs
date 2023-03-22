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
        public string StepName
        {
            get => txtMessage.Text;
            set => txtMessage.Text = value;
        }

        public FileInfo[] imageFiles;
        

        int curIndex;
        public int CurrentIndex
        {
            get => curIndex;
            set
            {
                if (value >= imageFiles.Length)
                    curIndex = 0;
                else if (value < 0)
                    curIndex = imageFiles.Length - 1;
                else
                    curIndex = value;

                if (curIndex < imageFiles.Length)
                {
                    try
                    {
                        mainImage.Source = new BitmapImage(new Uri(imageFiles[curIndex].FullName));
                        txtIndex.Text = $"{curIndex + 1}/{imageFiles.Length}";
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Error in loading image \r\n {ex.Message}");
                    }
                    timer.Duration = Properties.Settings.Default.ImageSliderDelay;
                }
                else
                {
                    timer.IsPaused = true;
                }
            }
        }

        Random r = new Random();
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
            timer.IsPaused = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Completed += (s,ev) => CurrentIndex++;
            CurrentIndex = r.Next(0, imageFiles.Length);
            btnMoveRight.Focus();
        }

        private void LeftRightCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void RightCommand_Executed(object sender, ExecutedRoutedEventArgs e) => CurrentIndex++;

        private void LeftCommand_Executed(object sender, ExecutedRoutedEventArgs e) => CurrentIndex--;
    }
}
