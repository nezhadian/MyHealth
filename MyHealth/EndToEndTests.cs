using IWshRuntimeLibrary;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MyHealth
{
    internal class EndToEndTests
    {
        public static void RunAll(MainWindow main)
        {

            if (TestSettingWindow())
            {
                if (TestColorDialogBox())
                {
                    main.tbNotify.Visibility = System.Windows.Visibility.Collapsed;

                    Console.WriteLine("All Tests Successfull");
                    Environment.Exit(0);
                }

            }

            Environment.Exit(99999);
        }

        private static bool TestColorDialogBox()
        {
            try
            {

                ColorDialog dialog = new ColorDialog();
                dialog.ShowDialog();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool TestSettingWindow()
        {
            try
            {
                SettingsWindow setWin = new SettingsWindow();
                setWin.Show();
                while (!setWin.IsLoaded) ;

                foreach (var item in setWin.MenuItems)
                {
                    if (item is ICanSaveSettingMenuItem)
                    {
                        ICanSaveSettingMenuItem canSave = (ICanSaveSettingMenuItem)item;
                        canSave.Save();
                    }
                }

                return true;
            }
            catch 
            {
                return false;
            }
            
            
        }
    }
}