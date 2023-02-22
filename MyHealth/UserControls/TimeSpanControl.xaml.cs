using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class TimeSpanControl : UserControl
    {
        public TimeSpan TimeSpan
        {
            set
            {
                txtHour.Text = value.Hours.ToString();
                txtMin.Text = value.Minutes.ToString();
                txtSecond.Text = value.Seconds.ToString();
            }
            get
            {
                if(int.TryParse(txtHour.Text,out int h))
                {
                    if (int.TryParse(txtMin.Text, out int m))
                    {
                        if (int.TryParse(txtSecond.Text, out int s))
                        {
                            return new TimeSpan(h, m, s);
                        }
                    }
                }
                return new TimeSpan(0);
                
            }
        }
        public TimeSpanControl()
        {
            InitializeComponent();
        }

        private void txtHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (Regex.IsMatch(textBox.Text, @"[0-9]{2}"))
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next) { 
                    Wrapped = true
                });
            }

        }

        private void TextBoxes_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtHour_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
