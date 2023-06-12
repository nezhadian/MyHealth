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
        PictureSlideShowViewModel PictureSlideShowViewModel;
        public PictureSlideShowStep()
        {
            InitializeComponent();
        }
        public PictureSlideShowStep(StepData step)
            : this()
        {
            Title = step.StepName;
            switch (step.ImageList)
            {
                case StepData.ImageListes.Body:
                    PictureSlideShowViewModel = new PictureSlideShowViewModel(App.GetAssetsPath("Images","body"));
                    break;
                case StepData.ImageListes.Eye:
                    PictureSlideShowViewModel = new PictureSlideShowViewModel(App.GetAssetsPath("Images", "eye"));
                    break;
            }
            DataContext = PictureSlideShowViewModel;
            PictureSlideShowViewModel.SelectionChanged += ViewModel_SelectionChanged;
        }

        private void ViewModel_SelectionChanged(object sender, RoutedEventArgs e)
        {
            tmrSlider.Reset();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            btnMoveRight.Focus();
            tmrSlider.IsPaused = true;
        }
        private void tmrSlider_Completed(object sender, RoutedEventArgs e)
        {
            Utils.RaiseCommand(PictureSlideShowViewModel.NextCommand, null);
        }
    }
}
