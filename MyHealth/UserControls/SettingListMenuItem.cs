using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyHealth
{
    public class SettingListMenuItem : ListBoxItem
    {


        public object ItemPage
        {
            get { return (object)GetValue(ItemPageProperty); }
            set { SetValue(ItemPageProperty, value); }
        }

        public static readonly DependencyProperty ItemPageProperty =
            DependencyProperty.Register("ItemPage", typeof(object), typeof(SettingListMenuItem), new PropertyMetadata());

        public SettingListMenuItem() { }
        public SettingListMenuItem(string name,Page page)
        {
            Content = name;
            ItemPage = page;
        }


    }
}
