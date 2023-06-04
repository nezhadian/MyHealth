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

        public TaskListViewModel TaskListViewModel { get; set; }
        public StepListViewModel StepListViewModel { get; set; }

        public MainWindow()
        {
            TaskListViewModel = new TaskListViewModel();
            StepListViewModel = new StepListViewModel();
            
            DataContext = this;
            InitializeComponent();
        }

        private void Timer_Completed(object sender, RoutedEventArgs e)
        {
            StepListViewModel.GoToNextStep();
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
        private void PagesContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StepListViewModel.ClickCommand.CanExecute(null))
                StepListViewModel.ClickCommand.Execute(null);
        }

        #region TaskBar Icon ContextMenu
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
            ExpandBeginStroyboard.Storyboard.Begin();
        }


        #endregion
    }
}
