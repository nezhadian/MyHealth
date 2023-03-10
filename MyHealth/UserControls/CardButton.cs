using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyHealth
{
    class CardButton : Button
    {


        public object UpperContent
        {
            get { return (object)GetValue(UpperContentProperty); }
            set { SetValue(UpperContentProperty, value); }
        }

        public static readonly DependencyProperty UpperContentProperty =
            DependencyProperty.Register("UpperContent", typeof(object), typeof(CardButton), new PropertyMetadata());


    }
}
