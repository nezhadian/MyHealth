using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for ShortBreak.xaml
    /// </summary>
    public partial class FreeTimeStep : Page
    {
        public FreeTimeStep()
        {
            DataContext = this;
            InitializeComponent();
        }

        public FreeTimeStep(StepData step)
            :this()
        {
            Title = step.StepName;
        }
    }
}
