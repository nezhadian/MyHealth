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
        #region StartAtStartup Property
        public static bool StartAtStartup
        {
            get => System.IO.File.Exists(GetStartupShortcutPath());
            set
            {
                if (value)
                    CreateStartupFile();
                else
                    RemoveStartupFile();
            }
        }
        private static void RemoveStartupFile()
        {
            FileInfo startupFile = new FileInfo(GetStartupShortcutPath());
            if (startupFile.Exists)
                startupFile.Delete();
        }
        private static void CreateStartupFile()
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(GetStartupShortcutPath());
            shortcut.Description = "this is my health startup item";
            shortcut.TargetPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location,".exe");
            shortcut.Save();

        }
        private static string GetStartupShortcutPath()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string filename = Assembly.GetExecutingAssembly().GetName().Name;
            return Path.ChangeExtension(Path.Combine(directory, filename), ".lnk");
        }
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        }

        public static string GetAssetsPath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, "Assets", path);
        }
        public static string GetAssetsPath(params string[] path)
        {
            return Path.Combine(new string[] { Environment.CurrentDirectory, "Assets" }.Concat(path).ToArray());
        }
    }
}
