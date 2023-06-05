using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MyHealth
{
    public class DockedWindow : Window
    {


        public object Dock
        {
            get { return (object)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register("Dock", typeof(object), typeof(DockedWindow), new PropertyMetadata());




        public object ConstantDockPart
        {
            get { return (object)GetValue(ConstantDockPartProperty); }
            set { SetValue(ConstantDockPartProperty, value); }
        }

        public static readonly DependencyProperty ConstantDockPartProperty =
            DependencyProperty.Register("ConstantDockPart", typeof(object), typeof(DockedWindow), new PropertyMetadata());




        public double DockMinHeight
        {
            get { return (double)GetValue(DockMinHeightProperty); }
            set { SetValue(DockMinHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DockMinHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockMinHeightProperty =
            DependencyProperty.Register("DockMinHeight", typeof(double), typeof(DockedWindow), new PropertyMetadata(550d));



        public double DockMaxHeight
        {
            get { return (double)GetValue(DockMaxHeightProperty); }
            set { SetValue(DockMaxHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DockMaxHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockMaxHeightProperty =
            DependencyProperty.Register("DockMaxHeight", typeof(double), typeof(DockedWindow), new PropertyMetadata(600d));



        public double DockMinWidth
        {
            get { return (double)GetValue(DockMinWidthProperty); }
            set { SetValue(DockMinWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DockMinWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockMinWidthProperty =
            DependencyProperty.Register("DockMinWidth", typeof(double), typeof(DockedWindow), new PropertyMetadata(650d));



        public double DockMaxWidth
        {
            get { return (double)GetValue(DockMaxWidthProperty); }
            set { SetValue(DockMaxWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DockMaxWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockMaxWidthProperty =
            DependencyProperty.Register("DockMaxWidth", typeof(double), typeof(DockedWindow), new PropertyMetadata(700d));









    }
}
