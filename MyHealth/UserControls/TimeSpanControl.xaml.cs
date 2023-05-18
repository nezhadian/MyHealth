using System;
using System.Collections.Generic;
using System.Globalization;
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
        #region Dp
        public static readonly DependencyProperty TimeSpanProperty =
            DependencyProperty.Register("TimeSpan", typeof(TimeSpan), typeof(TimeSpanControl), new PropertyMetadata(TimeSpan.Zero,OnTimeSpanChanged));
        public static readonly DependencyProperty HourVisibilityProperty =
            DependencyProperty.Register("HourVisibility", typeof(Visibility), typeof(TimeSpanControl), new PropertyMetadata(Visibility.Visible));
        #endregion
        public TimeSpan TimeSpan
        {
            get { return (TimeSpan)GetValue(TimeSpanProperty); }
            set { SetValue(TimeSpanProperty, value); }
        }
        public Visibility HourVisibility
        {
            get { return (Visibility)GetValue(HourVisibilityProperty); }
            set { SetValue(HourVisibilityProperty, value); }
        }

        //Events
        public event RoutedEventHandler TextChanged;

        //ctor
        public TimeSpanControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        //Static Dependency Properties Change Callbacks
        private static void OnTimeSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanControl tsc = (TimeSpanControl)d;
            TimeSpan timeSpan = (TimeSpan)e.NewValue;
            tsc.txtHour.Text = timeSpan.Hours.ToString();
            tsc.txtMin.Text = timeSpan.Minutes.ToString();
            tsc.txtSecond.Text = timeSpan.Seconds.ToString();
        }

        //TextBoxes Filters
        private void TextBoxOnlyNumbers_KeyDown(object sender, KeyEventArgs e)
        {
            int keyValue = (int)e.Key;
            bool isInNumbpad = keyValue >= 74 && keyValue <= 83;
            bool isInUpperNums = keyValue >= 34 && keyValue <= 43;
            bool isBackSpace = e.Key == Key.Back;
            bool isTab = e.Key == Key.Tab;
            e.Handled = !(isInNumbpad || isInUpperNums || isBackSpace || isTab);
        }
        private void TextBoxFilterMinMaxNumber59_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = int.TryParse(textBox.Text, out int n) ? Math.Max(Math.Min(n, 59), 0).ToString() : "0";
        }
        private void TextBoxFilterMinMaxNumber12_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = int.TryParse(textBox.Text, out int n) ? Math.Max(Math.Min(n, 12), 0).ToString() : "0";
        }
        private void TextBoxForwardFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 2)
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)
                {
                    Wrapped = true
                });
            }
            TimeSpan = GetTimeFromTextBoxes();
            TextChanged?.Invoke(this, null);

        }
        private TimeSpan GetTimeFromTextBoxes()
        {
            if (int.TryParse(txtHour.Text.ToString(), out int h))
            {
                if (int.TryParse(txtMin.Text.ToString(), out int m))
                {
                    if (int.TryParse(txtSecond.Text.ToString(), out int s))
                    {
                        return new TimeSpan(h, m, s);
                    }
                    return new TimeSpan(h, m, 0);
                }
                return new TimeSpan(h, 0, 0);
            }
            return new TimeSpan(0);
        }

        //Select All When Click
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

    }
}
