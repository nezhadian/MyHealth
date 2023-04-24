using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace MyHealth
{
    class AppSettings
    {
        public static MyHealthSettings Default = new MyHealthSettings();

        internal static void Save()
        {
            throw new NotImplementedException();
        }

        internal static void Reset()
        {
            throw new NotImplementedException();
        }
    }
    class MyHealthSettings
    {
        public Color FreshStartBgColor { get; internal set; }
        public Color ShortBreakBgColor { get; internal set; }
        public TimeSpan ImageSliderDelay { get; internal set; }
        public bool IsFirstRun { get; internal set; }
    }
}
