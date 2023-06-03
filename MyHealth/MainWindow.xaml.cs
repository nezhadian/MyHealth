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
        public static readonly DependencyProperty TimerProperty =
            DependencyProperty.Register("Timer", typeof(Timer), typeof(MainWindow), new PropertyMetadata());
        #endregion
        #region Properties
        //Task List
        public TaskListViewModel TaskListViewModel { get; set; }

        //Step List
        public StepListViewModel StepListViewModel { get; set; }

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
            StepListViewModel = new StepListViewModel();

            InitalizeTimer();
            
            DataContext = this;
            InitializeComponent();
        }

        #region Timer Initialization
        private void InitalizeTimer()
        {
            Timer = new Timer();
            Timer.Completed += Timer_Completed;
            BindingOperations.SetBinding(Timer, Timer.DurationProperty, new Binding("SelectedStep.Duration") { Source = StepListViewModel });
        }
        private void Timer_Completed(object sender, RoutedEventArgs e)
        {
            StepListViewModel.GoToNextStep();
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
