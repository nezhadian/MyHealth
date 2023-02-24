using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace MyHealth
{
    public class DataAccess
    {

        const string DATA_FILE_NAME = "StepData.dat";
        private static StepData[] steps;
        public static StepData[] StepDataList { 
            get => steps;
            set
            {
                steps = value;
                SaveData();
            } 
        }

        public static ITimerSlice[] GenerateSteps()
        {
            List<ITimerSlice> steps = new List<ITimerSlice>();
            foreach (StepData item in StepDataList)
            {
                var step = item.ToStep();
                if (step != null)
                    steps.Add(step);
            }
            return steps.ToArray();
        }

        public static void LoadData()
        {
            FileStream file = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(StepData[]));
                file = File.OpenRead($"{Environment.CurrentDirectory}\\{DATA_FILE_NAME}");
                steps = (StepData[])xml.Deserialize(file);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                steps = Templates.TemplateDictionary["standard"];
            }
            finally
            {
                file?.Close();
            }
        }

        public static void SaveData()
        {
            FileStream file = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(StepData[]));
                file = File.OpenWrite($"{Environment.CurrentDirectory}\\{DATA_FILE_NAME}");
                file.SetLength(0);
                xml.Serialize(file, StepDataList);
            }
            catch
            {
                MessageBox.Show("Can`t Save File");
            }
            finally
            {
                file?.Close();
            }
        }

    }
}
