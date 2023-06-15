using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

        //Privates
        static string DataFilePath;
        static bool isInitialized = false;

        //Events
        public static event RoutedEventHandler Initialized;

        //Methods
        public static void Init()
        {
            if (isInitialized)
                return;

            DataFilePath = Path.Combine(Environment.CurrentDirectory, "Settings.xml");
            Load();

            Initialized?.Invoke(null, null);
            isInitialized = true;
        }
        public static void InitAsync()
        {
            if (isInitialized)
                return;

            DataFilePath = Path.Combine(Environment.CurrentDirectory, "Settings.xml");
            Load();

            Application.Current.Dispatcher.Invoke(delegate { Initialized?.Invoke(null, null); });
            isInitialized = true;
        }

        static void Load()
        {
            FileStream stream = null;
            try
            {
                stream = File.OpenRead(DataFilePath);
                XmlSerializer xml = new XmlSerializer(Data.GetType());

                var loadedData = (MyHealthSettings)xml.Deserialize(stream);
                Data.SetData(loadedData);
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

        static CancellationTokenSource ctsSave;
        public static void SaveAsync()
        {
            ctsSave?.Cancel();
            ctsSave = new CancellationTokenSource();

            Thread thSave = new Thread(() => {

                CancellationToken token = ctsSave.Token;

                FileStream stream = null;
                XmlSerializer xml = new XmlSerializer(Data.GetType());
                try
                {
                    token.Register(() =>
                    {
                        stream?.Close();
                    });

                    stream = File.OpenWrite(DataFilePath);
                    xml.Serialize(stream, Data);
                    stream.SetLength(stream.Position);

                    Application.Current.Dispatcher.Invoke(Data.OnSaved);

                }
                finally
                {
                    stream?.Close();
                }

            });

            thSave.Start();
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
            {nameof(ShortBreakBgColor), new SettingItem(Color.FromRgb(0x00, 0x00, 0x00)) },
            {nameof(ImageSliderDelay), new SettingItem(TimeSpan.FromSeconds(20))},
            {nameof(GoOnTaskbar), new SettingItem(false) },
            {nameof(StartAtStartup), new SettingItem(true) },
        };

        //Settings
        public Color FreshStartBgColor
        {
            get => (Color)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }
        public Color ShortBreakBgColor
        {
            get => (Color)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }
        public TimeSpan ImageSliderDelay
        {
            get => (TimeSpan)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }

        public bool GoOnTaskbar
        {
            get => (bool)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }


        //dynamic Values
        public bool StartAtStartup
        {
            get => (bool)GetPropertyValue();
            set
            {
                SetPropertyValue(value);
            }
        }
        public override void OnLoaded()
        {
            base.OnLoaded();
            StartAtStartup = App.StartAtStartup;
        }
        public override void OnSaved()
        {
            base.OnSaved();
            App.StartAtStartup = StartAtStartup;
        }

        //Arrays
        private TaskView[] _taskList;
        public TaskView[] TaskList
        {
            get => _taskList ?? new TaskView[0];
            set
            {
                _taskList = value;
                OnPropertyChanged();
            }
        }

        private StepData[] _stepDataList;
        public StepData[] StepDataList
        {
            get => (StepData[])(_stepDataList ?? App.Current.TryFindResource("StepData.PomodoroTemplate"));
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

        protected virtual object GetPropertyValue([CallerMemberName] string propertyName = null)
        {
            if(Variables.TryGetValue(propertyName,out SettingItem item))
            {
                return item.Value;
            }
            return null;
        }
        protected virtual void SetPropertyValue(object value, [CallerMemberName] string propertyName = null)
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
        public virtual void OnSaved()
        {
            foreach (var item in Variables)
            {
                item.Value.ClearPreviousValue();
            }
        }


        public void SetData(SettingData data)
        {
            foreach (var item in data.Variables)
            {
                SetPropertyValue(item.Value.Value,item.Key);
            }
        }
        public virtual void OnLoaded()
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
