﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace MyHealth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
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

        public static bool IsTestMode;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (MyHealth.Properties.Settings.Default.IsFirstRun)
                ResetAllSettings();
            
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            IsTestMode = e.Args.Length == 1 && e.Args[0] == "test";
        }
        internal static void ResetAllSettings()
        {
            MyHealth.Properties.Settings.Default.Reset();
            StartAtStartup = true;
            MyHealth.Properties.Settings.Default.IsFirstRun = false;
            MyHealth.Properties.Settings.Default.Save();
        }

    }
}
