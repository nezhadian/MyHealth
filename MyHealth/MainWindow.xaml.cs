using GongSolutions.Wpf.DragDrop;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TaskbarIcon TaskBarIcon => ((MainWindow)App.Current.MainWindow).tbNotify;

        #region Dp
        public static readonly DependencyProperty StepListProperty =
            DependencyProperty.Register("StepList", typeof(ObservableCollection<StepData>), typeof(MainWindow), new PropertyMetadata());
        public static readonly DependencyProperty SelectedStepProperty =
            DependencyProperty.Register("SelectedStep", typeof(StepData), typeof(MainWindow), new PropertyMetadata());

        public static readonly DependencyProperty TimerProperty =
            DependencyProperty.Register("Timer", typeof(Timer), typeof(MainWindow), new PropertyMetadata());
        #endregion
        #region Properties
        //Task List
        public TaskListViewModel TaskListViewModel { get; set; }

        //Step List
        public ObservableCollection<StepData> StepList
        {
            get { return (ObservableCollection<StepData>)GetValue(StepListProperty); }
            set { SetValue(StepListProperty, value); }
        }
        public StepData SelectedStep
        {
            get { return (StepData)GetValue(SelectedStepProperty); }
            set { SetValue(SelectedStepProperty, value); }
        }

        //Timer
        public Timer Timer
        {
            get { return (Timer)GetValue(TimerProperty); }
            set { SetValue(TimerProperty, value); }
        }
        #endregion

        //Ctor
        public MainWindow()
        {
            TaskListViewModel = new TaskListViewModel();
            StepList = new ObservableCollection<StepData>(AppSettings.Data.StepDataList);
            
            AppSettings.Data.StepDataListChanged += StepDataListChanged;

            InitalizeTimer();
            
            DataContext = this;
            InitializeComponent();
        }

        #region StepList
        //Update StepList When Changed In Settings
        private void StepDataListChanged(object sender, RoutedEventArgs e)
        {
            StepList = new ObservableCollection<StepData>(AppSettings.Data.StepDataList);
            lstSteps.SelectedIndex = 0;
        }
        //Prevent Selecting Seperator Item in StepList
        private void lstSteps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStep != null && SelectedStep.StepType == StepData.StepTypes.Seperator)
                GoToNextStep();
        }
        //Slide Next When Click On FreshStart
        private void StepContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedStep.StepType == StepData.StepTypes.FreshStart)
                GoToNextStep();
        }
        #endregion
        #region Timer Initialization
        private void InitalizeTimer()
        {
            Timer = new Timer();
            Timer.Completed += Timer_Completed;
            BindingOperations.SetBinding(Timer, Timer.DurationProperty, new Binding("SelectedStep.Duration") { Source = this });
        }
        private void Timer_Completed(object sender, RoutedEventArgs e)
        {
            GoToNextStep();
        }
        private void GoToNextStep()
        {
            int curIndex = lstSteps.SelectedIndex;
            curIndex++;
            curIndex %= StepList.Count;
            lstSteps.SelectedIndex = curIndex;
        }
        #endregion
        #region Menu Events
        private void mnuReset_Click(object sender, RoutedEventArgs e)
        {
            Timer.Reset();
        }
        private void mnuSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.ShowDialog();
        }
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            tbNotify.Visibility = Visibility.Collapsed;
            Environment.Exit(0);
        }
        #endregion

        #region New Task Command And Textbox
        //Add New Task When Enter Pressed        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TaskListViewModel.AddTaskCommand.CanExecute(txtCommand.Text))
                {
                    TaskListViewModel.AddTaskCommand.Execute(txtCommand.Text);
                    txtCommand.Text = "";
                }
            }
        }
        #endregion

        #region TaskBar Icon ContextMenu
        //ContetMenu Init
        private void ContextMenu_Loaded(object sender, RoutedEventArgs e) => ContextMenuBindings();
        private void ContextMenuBindings()
        {
            mnuLockOnScreen.SetBinding(MenuItem.IsCheckedProperty, new Binding("Topmost")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });
        }

        //Items Events
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Activate();
            ExpandBeginStroyboard.Storyboard.Begin();
        }


        #endregion
        
    }
}
