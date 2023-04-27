using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyHealth
{
    public class SettingListMenuItem : DependencyObject
    {


        public Page ItemPage
        {
            get => (Page)GetValue(ItemPageProperty);
            set => SetValue(ItemPageProperty, value);
        }

        public static readonly DependencyProperty ItemPageProperty =
            DependencyProperty.Register("ItemPage", typeof(Page), typeof(SettingListMenuItem), new PropertyMetadata());

        public SettingListMenuItem(Page itemPage)
        {
            ItemPage = itemPage;
        }
    }
}
