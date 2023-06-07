using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace MyHealth
{
    class PictureSlideShowViewModel : ViewModelBase
    {
        public event RoutedEventHandler SelectionChanged;
        public ObservableCollection<BitmapImage> ImageList { get; set; }

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

        private int _selIndex;
        public int SelectedIndex
        {
            get => _selIndex;
            set
            {
                _selIndex = value;
                SelectedImage = ImageList[value];
                SelectionChanged?.Invoke(this, null);
                OnPropertyChanged();
            }
        }

        private string _imgDir;
        public string ImageDirectory
        {
            get => _imgDir;
            set
            {
                //Dirrectory Info
                DirectoryInfo dirInfo = new DirectoryInfo(value);

                //Check if exists
                if (!dirInfo.Exists)
                    throw new DirectoryNotFoundException();

                _imgDir = value;

                //Get Images In Directory
                ImageList = new ObservableCollection<BitmapImage>(
                    dirInfo.EnumerateFiles().Select((i) => new BitmapImage(new Uri(i.FullName))));

                //Set Properties
                SelectedIndex = new Random().Next(0, ImageList.Count);
                OnPropertyChanged();
            }
        }

        public PictureSlideShowNextCommand NextCommand { get; set; }
        public PictureSlideShowPreviousCommand PreviousCommand { get; set; }

        public PictureSlideShowViewModel(string imagePath)
        {
            ImageDirectory = imagePath;
            NextCommand = new PictureSlideShowNextCommand(this);
            PreviousCommand = new PictureSlideShowPreviousCommand(this);
        }

        public void GoToNextIndex()
        {
            int nextIndex = SelectedIndex;
            nextIndex++;
            if (nextIndex >= ImageList.Count)
                nextIndex = 0;
            SelectedIndex = nextIndex;
        }
        public void BackToPreviousIndex()
        {
            int previousIndex = SelectedIndex;
            previousIndex--;
            if (previousIndex < 0)
                previousIndex = ImageList.Count - 1;
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
