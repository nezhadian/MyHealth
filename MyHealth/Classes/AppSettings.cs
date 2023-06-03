using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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
            StepDataList = (StepData[])App.Current.Resources["StepData.PomodoroTemplate"],
            TaskList = new TaskView[0]
        };

        static string DataFilePath;
        static bool isInitialized = false;

        //Methods
        public static void Init()
        {
            if (isInitialized)
                return;

            DataFilePath = Path.Combine(Environment.CurrentDirectory, "Settings.xml");
            Load();

            isInitialized = true;
        }
        static void Load()
        {
            FileStream stream = null;
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
        public static void Reset()
        {
            Data = DEFAULT_DATA_VALUES.Clone();
        }
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
        
    }

    public class MyHealthSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region INotifyPropertyChanged Implamentation
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        //Props

        public Color FreshStartBgColor { get; set; }
        public Color ShortBreakBgColor { get; set; }
        public TimeSpan ImageSliderDelay { get; set; }
        public bool IsFirstRun { get; set; }
        public TaskView[] TaskList { set; get; }

        private StepData[] _stepDataList;
        public StepData[] StepDataList
        {
            get { return _stepDataList; }
            set
            {
                _stepDataList = value;
                OnPropertyChanged();
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
                StepDataList = StepDataList,
                TaskList = TaskList
            };
        }

    }
}
