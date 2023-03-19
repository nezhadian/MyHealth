﻿using System;
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
                        return new TimeSpan(h, m, 0);
                    }
                    return new TimeSpan(h, 0, 0);
                }
                return new TimeSpan(0);
                
            }
        }



        public Visibility HourVisibility
        {
            get { return (Visibility)GetValue(HourVisibilityProperty); }
            set { SetValue(HourVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HourVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HourVisibilityProperty =
            DependencyProperty.Register("HourVisibility", typeof(Visibility), typeof(TimeSpanControl), new PropertyMetadata(Visibility.Visible));



        public event RoutedEventHandler TextChanged;
        public TimeSpanControl()
        {
            InitializeComponent();
        }

        private void TextBoxOnlyNumbers_KeyDown(object sender, KeyEventArgs e)
        {
            int keyValue = (int)e.Key;
            bool isInNumbpad = keyValue >= 74 && keyValue <= 83;
            bool isInUpperNums = keyValue >= 34 && keyValue <= 43;
            bool isBackSpace = e.Key == Key.Back;
            e.Handled = !(isInNumbpad || isInUpperNums || isBackSpace);
        }

        private void TextBoxForwardFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 2)
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next) { 
                    Wrapped = true
                });
            }
            TextChanged?.Invoke(sender, e);

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
