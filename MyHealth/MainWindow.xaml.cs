using AdonisUI;
using GongSolutions.Wpf.DragDrop;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DockedWindow
    {
        public static TaskbarIcon TaskBarIcon => ((MainWindow)App.Current.MainWindow).tbNotify;

        public TaskListViewModel TaskListViewModel { get; set; }
        public StepListViewModel StepListViewModel { get; set; }

        SettingsWindow settingsWin = new SettingsWindow();

        public MainWindow()
        {

            TaskListViewModel = new TaskListViewModel();
            StepListViewModel = new StepListViewModel();

            if (AppSettings.IsInitialized)
                AppSettings_Initialized(null, null);
            else
                AppSettings.Initialized += AppSettings_Initialized;

            AppSettings.Data.PropertyChanged += AppSettings_Data_PropertyChanged;

            DataContext = this;
            InitializeComponent();
            
            //
        }

        private void AppSettings_Initialized(object sender, RoutedEventArgs e)
        {
            TaskListViewModel.AddArrayToTaskList(AppSettings.Data.TaskList);
            StepListViewModel.StepsArray = AppSettings.Data.StepDataList;
        }
        private void AppSettings_Data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppSettings.Data.StepDataList))
                StepListViewModel.StepsArray = AppSettings.Data.StepDataList;

            else if (e.PropertyName == nameof(AppSettings.Data.LanguageCode))
                LanguageSelector.SetLanguage(AppSettings.Data.LanguageCode);

            else if (e.PropertyName == nameof(AppSettings.Data.IsDarkMode))
            {
                //ResourceLocator.SetColorScheme(Application.Current.Resources, AppSettings.Data.IsDarkMode ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme);
                //ResourceDictionary rs = Application.Current.Resources;

                //MyAdonisResourceLocator.SetColorScheme(AppSettings.Data.IsDarkMode ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme);

                //Application.Current.Resources.MergedDictionaries[0].Source = AppSettings.Data.IsDarkMode ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme;
            }




        }


        private void Timer_Completed(object sender, RoutedEventArgs e)
        {
            StepListViewModel.GoToNextStep();
        }

        private void mnuSettings_Click(object sender, RoutedEventArgs e)
        {
            settingsWin.Show();
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
                if(Utils.RaiseCommand(TaskListViewModel.AddTaskCommand, txtCommand.Text))
                {
                    txtCommand.Text = "";
                }
            }
        }
        private void PagesContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.RaiseCommand(StepListViewModel.ClickCommand, null);
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
        }


        #endregion
    }
}
