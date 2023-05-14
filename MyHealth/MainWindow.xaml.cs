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

        public Timer Timer
        {
            get { return (Timer)GetValue(TimerProperty); }
            set { SetValue(TimerProperty, value); }
        }


        public MainWindow()
        {
            TaskList = new ObservableCollection<TaskView>();
            StepList = new ObservableCollection<StepData>(Templates.TemplateDictionary["pomodoro"]);
            InitalizeTimer();
            
            DataContext = this;
            InitializeComponent();
            
        }


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

        

        #region Add TextBox
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ApplicationCommands.New.CanExecute(null, null))
                    ApplicationCommands.New.Execute(null, null);
            }
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrWhiteSpace(txtCommand.Text);

        }
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TaskList.Add(new TaskView()
            {
                Title = txtCommand.Text
            });
            txtCommand.Text = "";
        }
        #endregion
        #region TaskBar Icon Menu
        private void ContextMenu_Loaded(object sender, RoutedEventArgs e) => ContextMenuBindings();
        private void ContextMenuBindings()
        {
            mnuLockOnScreen.SetBinding(MenuItem.IsCheckedProperty, new Binding("Topmost")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            });


        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Activate();
            MouseEnterAnimation.Storyboard.Begin();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //bool topmost = Topmost;
            //Topmost = false;
            //timer.IsPaused = true;

            //var settingsWin = new SettingsWindow();
            //if(true == settingsWin.ShowDialog())
            //{
            //    if (DataAccess.IsStepListChanged)
            //    {
            //        LoadSteps();
            //        DataAccess.IsStepListChanged = false;
            //    }
            //}

            //timer.IsPaused = false;
            //Topmost = topmost;
        }
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            tbNotify.Visibility = Visibility.Collapsed;
            Environment.Exit(0);
        }

        #endregion


    }

    public interface ITimerSlice 
    {
        public TimeSpan Duration { set; get; }
        public bool RequireClick { set; get; }
        public string StepName { set; get; }
    }


}
