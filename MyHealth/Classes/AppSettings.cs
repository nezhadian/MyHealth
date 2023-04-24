using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        };

        //ctor
        static AppSettings()
        {
            try
            {
                Task.Run(Load);
            }
            catch
            {
                Reset();
            }
        }

        //Methods
        public static void Save()
        {
            Task.Run(() =>
            {
                XmlSerializer xml = new XmlSerializer(Data.GetType());
                FileStream stream = File.OpenWrite("G:\\MyHealth.Data");

                stream.SetLength(0);
                xml.Serialize(stream, Data);
                stream.Close();
            });
        }
        public static void Reset()
        {
            Data = DEFAULT_DATA_VALUES.Clone();
        }
        static void Load()
        {
            XmlSerializer xml = new XmlSerializer(Data.GetType());
            FileStream stream = File.OpenRead("G:\\MyHealth.Data");

            Data = (MyHealthSettings)xml.Deserialize(stream);
            stream.Close();
        }
    }

    public class MyHealthSettings 
    {
        //Props
        public Color FreshStartBgColor { get; set; }
        public Color ShortBreakBgColor { get; set; }
        public TimeSpan ImageSliderDelay { get; set; }
        public bool IsFirstRun { get; set; }

        //Methods
        public MyHealthSettings Clone()
        {
            return new MyHealthSettings()
            {
                FreshStartBgColor = FreshStartBgColor,
                ShortBreakBgColor = ShortBreakBgColor,
                ImageSliderDelay = ImageSliderDelay,
                IsFirstRun = IsFirstRun
            };
        }

    }
}
