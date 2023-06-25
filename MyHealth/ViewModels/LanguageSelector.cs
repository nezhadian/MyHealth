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
        public static LanguageSelector Instance;


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



        static ResourceDictionaryLocator languageFileChanger;
        public static void SetLanguage(string lang)
        {
            //change lanuage dictonary file
            languageFileChanger.ChangeFile(lang + ".xaml");

            //change application cultue
            CultureInfo culture = new CultureInfo(lang);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            //change application direction
            Instance.ContentFlowDirection = culture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        public LanguageSelector()
        {
            languageFileChanger = new ResourceDictionaryLocator("/Languages/en-us.xaml");
        }

        static LanguageSelector()
        {
            Instance = new LanguageSelector();
        }
    }
}
