using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace MyHealth
{
    class PictureSlideShowViewModel : ViewModelBase
    {
        //Events
        public event RoutedEventHandler SelectionChanged;

        //Props

        private BitmapImage[] _imgArray;
        public BitmapImage[] ImageArray
        {
            get => _imgArray;
            set
            {
                _imgArray = value;
                OnPropertyChanged();

                if(value.Length > 0)
                    SelectedIndex = new Random().Next(0, ImageArray.Length);

            }
        }

        private int _selIndex;
        public int SelectedIndex
        {
            get => _selIndex;
            set
            {
                _selIndex = value;
                SelectedImage = ImageArray?[value];
                SelectionChanged?.Invoke(this, null);
                OnPropertyChanged();
            }
        }

        private BitmapImage _selImg;
        public BitmapImage SelectedImage
        {
            get => _selImg;
            set
            {
                _selImg = value;
                OnPropertyChanged();
            }
        }

        //Commands
        public PictureSlideShowNextCommand NextCommand { get; set; }
        public PictureSlideShowPreviousCommand PreviousCommand { get; set; }

        //Privates Statics
        public PictureSlideShowViewModel(string dirPath)
        {
            ImageArray = new BitmapImage[0];
            NextCommand = new PictureSlideShowNextCommand(this);
            PreviousCommand = new PictureSlideShowPreviousCommand(this);

            LoadDirectoryAsync(dirPath);
        }

        //Load Images
        private void LoadDirectoryAsync(string dirPath)
        {

            if (!Directory.Exists(dirPath))
                throw new DirectoryNotFoundException();

            Task.Run(() =>
            {
                
                BitmapImage[] imgs = Directory.GetFiles(dirPath).Select((path) => LoadImage(path)).ToArray();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ImageArray = imgs;
                });


            });
        }

        static Dictionary<string, BitmapImage> cache = new Dictionary<string, BitmapImage>();
        private BitmapImage LoadImage(string imagePath)
        {
            if (cache.TryGetValue(imagePath, out BitmapImage img))
                return img;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.DecodePixelWidth = 600;
            bi.CacheOption = BitmapCacheOption.None;
            bi.UriSource = new Uri(imagePath);
            bi.EndInit();
            bi.Freeze();

            cache.Add(imagePath, bi);
            return bi;
        }

        //Navigation
        public void GoToNextIndex()
        {
            if (ImageArray.Length == 0)
                return;
            
            int nextIndex = SelectedIndex;
            nextIndex++;
            if (nextIndex >= ImageArray.Length)
                nextIndex = 0;
            SelectedIndex = nextIndex;
        }
        public void BackToPreviousIndex()
        {
            if (ImageArray.Length == 0)
                return;

            int previousIndex = SelectedIndex;
            previousIndex--;
            if (previousIndex < 0)
                previousIndex = ImageArray.Length - 1;
            SelectedIndex = previousIndex;
        }
    }

    class PictureSlideShowNextCommand : ContextCommand<PictureSlideShowViewModel>
    {
        public PictureSlideShowNextCommand(PictureSlideShowViewModel context) : base(context) { }

        public override bool CanExecute(PictureSlideShowViewModel context, object parameter)
        {
            return true;
        }

        public override void Execute(PictureSlideShowViewModel context, object parameter)
        {
            context.GoToNextIndex();
        }
    }
    class PictureSlideShowPreviousCommand : ContextCommand<PictureSlideShowViewModel>
    {
        public PictureSlideShowPreviousCommand(PictureSlideShowViewModel context) : base(context) { }

        public override bool CanExecute(PictureSlideShowViewModel context, object parameter)
        {
            return true;
        }

        public override void Execute(PictureSlideShowViewModel context, object parameter)
        {
            context.BackToPreviousIndex();
        }
    }
}
