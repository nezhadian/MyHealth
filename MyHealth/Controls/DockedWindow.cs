using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MyHealth
{
    public class DockedWindow : Window
    {
        #region Dp
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register("Dock", typeof(object), typeof(DockedWindow), new PropertyMetadata());
        public static readonly DependencyProperty ConstantDockPartProperty =
            DependencyProperty.Register("ConstantDockPart", typeof(object), typeof(DockedWindow), new PropertyMetadata());
        public static readonly DependencyProperty DockMinHeightProperty =
            DependencyProperty.Register("DockMinHeight", typeof(double), typeof(DockedWindow), new PropertyMetadata(550d));
        public static readonly DependencyProperty DockMaxHeightProperty =
            DependencyProperty.Register("DockMaxHeight", typeof(double), typeof(DockedWindow), new PropertyMetadata(600d));
        public static readonly DependencyProperty DockMinWidthProperty =
            DependencyProperty.Register("DockMinWidth", typeof(double), typeof(DockedWindow), new PropertyMetadata(650d));
        public static readonly DependencyProperty DockMaxWidthProperty =
            DependencyProperty.Register("DockMaxWidth", typeof(double), typeof(DockedWindow), new PropertyMetadata(700d));
        #endregion

        public object Dock
        {
            get { return (object)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }
        public object ConstantDockPart
        {
            get { return (object)GetValue(ConstantDockPartProperty); }
            set { SetValue(ConstantDockPartProperty, value); }
        }

        public double DockMinHeight
        {
            get { return (double)GetValue(DockMinHeightProperty); }
            set { SetValue(DockMinHeightProperty, value); }
        }
        public double DockMaxHeight
        {
            get { return (double)GetValue(DockMaxHeightProperty); }
            set { SetValue(DockMaxHeightProperty, value); }
        }

        public double DockMinWidth
        {
            get { return (double)GetValue(DockMinWidthProperty); }
            set { SetValue(DockMinWidthProperty, value); }
        }
        public double DockMaxWidth
        {
            get { return (double)GetValue(DockMaxWidthProperty); }
            set { SetValue(DockMaxWidthProperty, value); }
        }
    }
}
