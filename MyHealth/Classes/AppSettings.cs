using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MyHealth
{
    class AppSettings
    {
        //Props
        public static MyHealthSettings Data = new MyHealthSettings();
        public static readonly MyHealthSettings DEFAULT_DATA_VALUES = new MyHealthSettings()
        {
            ImageSliderDelay = new TimeSpan(0, 0, 20),
            FreshStartBgColor = Color.FromRgb(0x00, 0x80, 0x00),
            ShortBreakBgColor = Color.FromRgb(0x00, 0x00, 0x00),
            IsFirstRun = true,
            StepDataList = Templates.TemplateDictionary["pomodoro"]
        };

        static string DataFilePath => Path.Combine(Environment.CurrentDirectory, "Settings.xml");

        //ctor
        static AppSettings()
        {
            Load();
        }

        //Methods
        public static void Save()
        {
            FileStream stream = null;
            XmlSerializer xml = new XmlSerializer(Data.GetType());
            try
            {
                stream = File.OpenWrite(DataFilePath);
                stream.SetLength(0);
                xml.Serialize(stream, Data);

            }
            finally
            {
                stream?.Close();
            }
        }
        public static void Reset()
        {
            Data = DEFAULT_DATA_VALUES.Clone();
        }
        static void Load()
        {
            FileStream stream = null ;
            try
            {
                stream = File.OpenRead(DataFilePath);
                XmlSerializer xml = new XmlSerializer(Data.GetType());

                Data = (MyHealthSettings)xml.Deserialize(stream);
            }
            catch
            {
                Reset();
            }
            finally
            {
                stream?.Close();
            }
        }
    }

    public class MyHealthSettings 
    {
        //Props
        public Color FreshStartBgColor { get; set; }
        public Color ShortBreakBgColor { get; set; }
        public TimeSpan ImageSliderDelay { get; set; }
        public bool IsFirstRun { get; set; }


        public event RoutedEventHandler StepDataListChanged;
        
        private StepData[] _stepDataList;
        public StepData[] StepDataList
        {
            get { return _stepDataList; }
            set { 
                _stepDataList = value;
                StepDataListChanged?.Invoke(this, null);
            }
        }

        

        //Methods
        public MyHealthSettings Clone()
        {
            return new MyHealthSettings()
            {
                FreshStartBgColor = FreshStartBgColor,
                ShortBreakBgColor = ShortBreakBgColor,
                ImageSliderDelay = ImageSliderDelay,
                IsFirstRun = IsFirstRun,
                StepDataList = StepDataList
            };
        }

    }
}
