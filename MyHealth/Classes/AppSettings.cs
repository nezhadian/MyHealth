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
                Data.OnLoaded();
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
            Data.ResetValues();
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

                Data.OnSaved();

            }
            finally
            {
                stream?.Close();
            }
        }

        public static void UndoAll()
        {
            Data.UndoChanges();
        }
        
    }

    public class MyHealthSettings : SettingData
    {
        protected override Dictionary<string, SettingItem> Variables { get; set; } = new Dictionary<string, SettingItem>()
        {
            {nameof(FreshStartBgColor), new SettingItem(Color.FromRgb(0x00, 0x80, 0x00)) },
        };

        public Color FreshStartBgColor
        {
            get => (Color)GetMyValue();
            set
            {
                SetMyValue(value);
            }
        }

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
    }

    public abstract class SettingData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implamentation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        [XmlIgnore]
        protected abstract Dictionary<string, SettingItem> Variables { set; get; }

        protected virtual object GetMyValue([CallerMemberName] string propertyName = null)
        {
            if(Variables.TryGetValue(propertyName,out SettingItem item))
            {
                return item.Value;
            }
            return null;
        }
        protected virtual void SetMyValue(object value, [CallerMemberName] string propertyName = null)
        {
            if (Variables.TryGetValue(propertyName, out SettingItem item))
            {
                Variables[propertyName].Value = value;
            }
            else
            {
                Variables.Add(propertyName, new SettingItem(value));
            }
            OnPropertyChanged(propertyName);
        }

        public void OnSaved()
        {
            foreach (var item in Variables)
            {
                item.Value.ClearPreviousValue();
            }
        }
        public void ResetValues()
        {
            foreach (var item in Variables)
            {
                item.Value.ResetToDefault();
                OnPropertyChanged(item.Key);
            }
        }
        public void UndoChanges()
        {
            foreach (var item in Variables)
            {
                item.Value.Undo();
                OnPropertyChanged(item.Key);
            }
        }
        public void OnLoaded()
        {
            foreach (var item in Variables)
            {
                item.Value.Init();
            }
        }
    }
    public class SettingItem
    {
        public readonly object Default;

        public object Value;
        public object Previous;

        public SettingItem() { }
        public SettingItem(object defaultValue)
        {
            Default = defaultValue;
            Value = defaultValue;
            Previous = defaultValue;

        }

        public void ClearPreviousValue()
        {
            Previous = Value;
        }
        public void ResetToDefault()
        {
            Value =  Default;
        }
        public void Undo()
        {
            Value = Previous;
        }
        public void Init()
        {
            Previous = Value;
        }
    }
}
