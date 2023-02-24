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

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for ShortBreak.xaml
    /// </summary>
    public partial class ShortBreak : Page
    {
 
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MONITORPOWER = 0xF170;
        const int MONITOR_ON = 1;
        const int MONITOR_OFF = 2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int hMsg, int wParam, int lParam);


        public ShortBreak()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SendMessage(Process.GetCurrentProcess().Handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            SendMessage(Process.GetCurrentProcess().Handle, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_ON);
        }
    }
}
