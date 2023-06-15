using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace MyHealth
{
    class AppSettings
    {
        //Props
        public static MyHealthSettings Data = new MyHealthSettings();

        //Privates
        static string dataFilePath;

        //Loading
        public static bool IsInitialized { get; set; }
        public static event RoutedEventHandler Initialized;

        public static void Init()
        {
            Task.Run(InitAsync);
        }
        static void InitAsync()
        {
            if (IsInitialized)
                return;

            dataFilePath = Path.Combine(Environment.CurrentDirectory, "Settings.xml");
            Load();

            Application.Current.Dispatcher.Invoke(delegate { Initialized?.Invoke(null, null); });
            IsInitialized = true;
        }
        static void Load()
        {
            FileStream stream = null;
            try
            {
                stream = File.OpenRead(dataFilePath);
                XmlSerializer xml = new XmlSerializer(Data.GetType());

                var loadedData = (MyHealthSettings)xml.Deserialize(stream);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utils.MatchPropertiesValue(loadedData, Data);
                });

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

        //Methods
        public static void Reset()
        {
            Data.ResetValues();
        }
        public static void UndoAll()
        {
            Data.UndoChanges();
        }

        //Save
        static CancellationTokenSource ctsSave;
        public static void SaveAsync()
        {
            ctsSave?.Cancel();
            ctsSave = new CancellationTokenSource();

            Task.Run(() => {

                CancellationToken token = ctsSave.Token;

                FileStream stream = null;
                XmlSerializer xml = new XmlSerializer(Data.GetType());
                try
                {
                    token.Register(() =>
                    {
                        stream?.Close();
                    });

                    stream = File.OpenWrite(dataFilePath);
                    xml.Serialize(stream, Data);
                    stream.SetLength(stream.Position);

                    Application.Current.Dispatcher.Invoke(Data.OnSaved);

                }
                finally
                {
                    stream?.Close();
                }

            });
        }



    }
}
