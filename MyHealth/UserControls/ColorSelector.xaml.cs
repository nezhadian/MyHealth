using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : System.Windows.Controls.UserControl
    {
        #region Dp
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ColorSelector), new PropertyMetadata());
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorSelector), new PropertyMetadata(Colors.Red));
        #endregion

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public event RoutedEventHandler ColorChanged;

        public ColorSelector()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = System.Drawing.Color.FromArgb(Color.A,Color.R,Color.G,Color.B);
            if(DialogResult.OK == dialog.ShowDialog())
            {
                Color = Color.FromArgb(dialog.Color.A,dialog.Color.R, dialog.Color.G, dialog.Color.B);
                ColorChanged?.Invoke(this, new RoutedEventArgs());
            }
        }
    }
}
