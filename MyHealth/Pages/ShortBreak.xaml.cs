using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for ShortBreak.xaml
    /// </summary>
    public partial class ShortBreak : Page,ITimerSlice
    {
        public TimeSpan Duration { get ; set ; }
        public bool RequireClick { get; set; }
        public string StepName
        {
            set => txtMessage.Text = value;
            get => txtMessage.Text;
        }

        public ShortBreak()
        {
            InitializeComponent();
        }
        public ShortBreak(TimeSpan duration)
        {
            InitializeComponent();
            Duration = duration;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TurnOffMonitor();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TurnOnMonitor();
        }

        #region Monitor Control

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MONITORPOWER = 0xF170;
        const int HWND_BROADCAST = -1;
        const int MONITOR_OFF = 2;
        private void TurnOffMonitor()
        {
            SendMessage(HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private void TurnOnMonitor()
        {
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
        }

        #endregion
    }
}
