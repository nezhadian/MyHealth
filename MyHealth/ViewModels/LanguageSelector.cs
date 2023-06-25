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
    public class LanguageSelector : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Implamentation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        private FlowDirection _flwDirection;
        public FlowDirection ContentFlowDirection
        {
            get => _flwDirection;
            set
            {
                _flwDirection = value;
                OnPropertyChanged();
            }
        }

        public static LanguageSelector Instance;

        static ResourceDictionary languageDictornary;
        public static void SetLanguage(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);

            languageDictornary.Source = new Uri($"/Languages/{lang}.xaml", UriKind.Relative);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            Instance.ContentFlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        static LanguageSelector()
        {
            languageDictornary = App.Current.Resources.MergedDictionaries.First((rs) => rs.Source.OriginalString == "/Languages/en-us.xaml");
            Instance = new LanguageSelector();
        }
    }
}
