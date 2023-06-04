using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for SocialLinkCardButton.xaml
    /// </summary>
    public class SocialLinkCardButton : Button
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SocialLinkCardButton), new PropertyMetadata());


        public Geometry IconPathData
        {
            get { return (Geometry)GetValue(IconPathDataProperty); }
            set { SetValue(IconPathDataProperty, value); }
        }
        public static readonly DependencyProperty IconPathDataProperty =
            DependencyProperty.Register("IconPathData", typeof(Geometry), typeof(SocialLinkCardButton), new PropertyMetadata());



        public Uri Link
        {
            get { return (Uri)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(Uri), typeof(SocialLinkCardButton), new PropertyMetadata());
    }
}
