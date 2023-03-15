using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using IWshRuntimeLibrary;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MyHealth.Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "StartAtStartup")
            {
                bool StartAtStartup = MyHealth.Properties.Settings.Default.StartAtStartup;
                if (StartAtStartup)
                {
                    CreateStartupFile();
                }
                else
                {
                    RemoveStartupFile();
                }

            }
            
        }

        private void RemoveStartupFile()
        {
            FileInfo startupFile = new FileInfo(GetStartupShortcutPath());
            if (startupFile.Exists)
                startupFile.Delete();
        }
        private void CreateStartupFile()
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(GetStartupShortcutPath());
            shortcut.Description = "this is my health startup item";
            shortcut.TargetPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location,".exe");
            shortcut.Save();

        }
        string GetStartupShortcutPath()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string filename = Assembly.GetExecutingAssembly().GetName().Name;
            return Path.ChangeExtension(Path.Combine(directory, filename), ".lnk");
        }
    }
}
