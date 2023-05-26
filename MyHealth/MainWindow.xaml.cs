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
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<TaskView>), typeof(MainWindow), new PropertyMetadata());
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(TaskView), typeof(MainWindow), new PropertyMetadata());



        public static readonly DependencyProperty StepListProperty =
            DependencyProperty.Register("StepList", typeof(ObservableCollection<StepData>), typeof(MainWindow), new PropertyMetadata());
        public static readonly DependencyProperty SelectedStepProperty =
            DependencyProperty.Register("SelectedStep", typeof(StepData), typeof(MainWindow), new PropertyMetadata());

        public static readonly DependencyProperty TimerProperty =
            DependencyProperty.Register("Timer", typeof(Timer), typeof(MainWindow), new PropertyMetadata());
        #endregion
        #region Properties
        //Task List
        public ObservableCollection<TaskView> TaskList
        {
            get { return (ObservableCollection<TaskView>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public TaskView SelectedTask
        {
            get { return (TaskView)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

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
            TaskList = new ObservableCollection<TaskView>(AppSettings.Data.TaskList ?? new TaskView[0]);
            StepList = new ObservableCollection<StepData>(AppSettings.Data.StepDataList);
            
            AppSettings.Data.StepDataListChanged += StepDataListChanged; ;
            Array.ForEach(AppSettings.Data.TaskList, (i) => i.PropertyChanged += Task_PropertyChanged);

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
        #region New Task 
        //Add New Task When Enter Pressed        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ApplicationCommands.New.CanExecute(null, null))
                    ApplicationCommands.New.Execute(null, null);
            }
        }

        //New Command
        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrWhiteSpace(txtCommand.Text);

        }
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TaskView newTask = new TaskView()
                {Title = txtCommand.Text };
            newTask.PropertyChanged += Task_PropertyChanged;

            TaskList.Add(newTask);
            txtCommand.Text = "";

            SaveTaskList();
        }
        #endregion
        #region TaskList Saving
        //Save Task List
        bool NeedSave = false;
        //Need Save an Item Changed
        private void Task_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NeedSave = true;
        }
        //Save If Needed When TaskList Selection Change Or Focus Changed
        private void lstTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveTaskListIfNeeded();

        }
        private void lstTasks_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveTaskListIfNeeded();
        }
        //Saving Methods
        private void SaveTaskListIfNeeded()
        {
            if (NeedSave)
            {
                SaveTaskList();
                NeedSave = false;
            }
        }
        private void SaveTaskList()
        {
            AppSettings.Data.TaskList = TaskList.ToArray();
            AppSettings.Save();
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
