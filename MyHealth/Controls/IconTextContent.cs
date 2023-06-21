using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyHealth
{
    public class IconTextContent:ContentControl
    {


        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value);}
        }

        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(Geometry), typeof(IconTextContent), new PropertyMetadata());


    }
}
