using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public partial class PictureSlideShowStep : Page
    {
        #region Dp
        public static readonly DependencyProperty ImageListProperty =
            DependencyProperty.Register("ImageList", typeof(ObservableCollection<BitmapImage>), typeof(PictureSlideShowStep), new PropertyMetadata(new ObservableCollection<BitmapImage>()));
        public static readonly DependencyProperty SelectedImageProperty =
            DependencyProperty.Register("SelectedImage", typeof(BitmapImage), typeof(PictureSlideShowStep), new PropertyMetadata());
        #endregion

        public ObservableCollection<BitmapImage> ImageList
        {
            get { return (ObservableCollection<BitmapImage>)GetValue(ImageListProperty); }
            set { SetValue(ImageListProperty, value); }
        }
        public BitmapImage SelectedImage
        {
            get { return (BitmapImage)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }

        private string imgDir;
        public string ImagesDirectory
        {
            get { return imgDir; }
            set { 
                imgDir = value;
                //Dirrectory Info
                DirectoryInfo dirInfo = new DirectoryInfo(value);

                //Check if exists
                if (!dirInfo.Exists)
                    throw new DirectoryNotFoundException();

                //Get Images In Directory
                ImageList = new ObservableCollection<BitmapImage>(
                    dirInfo.EnumerateFiles().Select((i) => new BitmapImage(new Uri(i.FullName))));

                //Set Properties
                lstImages.SelectedIndex = FindRandomIndex();
            }
        }

        DispatcherTimer timer;
        Random r = new Random();

        public PictureSlideShowStep()
        {
            timer = new DispatcherTimer
            {
                Interval = AppSettings.Data.ImageSliderDelay
            };
            timer.Tick += SlideShow_Tick;
            
            
            DataContext = this;
            InitializeComponent();
        }

        private void SlideShow_Tick(object sender, EventArgs e)
        {
            ComponentCommands.MoveRight.Execute(null,null);
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private int FindNextIndex()
        {
            int selIndex = lstImages.SelectedIndex;
            selIndex++;
            if (selIndex >= ImageList.Count)
                return 0;
            else
                return selIndex;
        }
        private int FindRandomIndex()
        {
            return r.Next(0, ImageList.Count);
        }
        private int FindPreviousIndex()
        {
            int selIndex = lstImages.SelectedIndex;
            selIndex--;
            if (selIndex < 0)
                return ImageList.Count - 1;
            else
                return selIndex;
        }

        private void Always_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void NextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            lstImages.SelectedIndex = FindNextIndex();
        }
        private void PreviousCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            lstImages.SelectedIndex = FindPreviousIndex();

        }
        
        public PictureSlideShowStep(StepData step)
            : this()
        {
            Title = step.StepName;
            switch (step.ImageList)
            {
                case StepData.ImageListes.Body:
                    ImagesDirectory = App.GetAssetsPath("Images","body");
                    break;
                case StepData.ImageListes.Eye:
                    ImagesDirectory = App.GetAssetsPath("Images","eye");
                    break;
            }
        }

    }
}
