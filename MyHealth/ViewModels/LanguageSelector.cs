using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MyHealth
{
    public class LanguageSelector 
    {
        public static LanguageSelector Instance;

        static ResourceDictionary languageDictornary;
        public LanguageSelector()
        {
            languageDictornary = App.Current.Resources.MergedDictionaries.First((rs) => rs.Source.OriginalString == "/Languages/en-us.xaml");
        }
        public static void SetLanguage(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);

            languageDictornary.Source = new Uri($"/Languages/{lang}.xaml", UriKind.Relative);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        static LanguageSelector()
        {
            languageDictornary = App.Current.Resources.MergedDictionaries.First((rs) => rs.Source.OriginalString == "/Languages/en-us.xaml");
            Instance = new LanguageSelector();
        }
    }
}
