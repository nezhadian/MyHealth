using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHealth
{
    public partial class TimeSpanControlStyle : ResourceDictionary
    {
        public TimeSpanControlStyle()
        {
            InitializeComponent();
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
