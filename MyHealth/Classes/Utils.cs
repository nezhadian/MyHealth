using AdonisUI.Controls;
using System;
using System.Windows.Input;

namespace MyHealth
{
    internal class Utils
    {
        public static void InfoMessageBox(string caption,string message,string okLabel = "Ok")
        {
            MessageBoxModel model = new MessageBoxModel();
            model.Buttons = new MessageBoxButtonModel[]
            {
                new MessageBoxButtonModel(okLabel,MessageBoxResult.OK),
            };

            model.Text = message;
            model.Caption = caption;

            MessageBox.Show(model);

        }
        public static void InfoMessageBoxFromResources(string key)
        {
            InfoMessageBox(
                 GetTextResource($"MessageBox.{key}.Caption"),
                 GetTextResource($"MessageBox.{key}.Text"),
                 GetTextResource($"MessageBox.{key}.OKLabel"));
        }
        public static void ErrorMessageBox(Exception ex)
        {
            InfoMessageBox("Error",ex.ToString(),"OK");
        }


        public static bool YesNoMessageBox(string caption, string message,string yesLabel = "Yes",string noLabel = "No")
        {
            MessageBoxModel model = new MessageBoxModel();
            model.Buttons = new MessageBoxButtonModel[]
            {
                new MessageBoxButtonModel(yesLabel,MessageBoxResult.Yes),
                new MessageBoxButtonModel(noLabel,MessageBoxResult.No),
            };

            model.Text = message;
            model.Caption = caption;
            
            return MessageBoxResult.Yes == MessageBox.Show(model);
        }
        public static bool YesNoMessageBoxFromResources(string key)
        {
            return YesNoMessageBox(
                GetTextResource($"MessageBox.{key}.Caption"),
                GetTextResource($"MessageBox.{key}.Text"),
                GetTextResource($"MessageBox.{key}.YesLabel"),
                GetTextResource($"MessageBox.{key}.NoLabel"));
        }


        public static bool RaiseCommand(ICommand command,object param)
        {
            if (command.CanExecute(param))
            {
                command.Execute(param);
                return true;
            }
            return false;
        }
        internal static void MatchPropertiesValue<T>(T sourceObject, T targetObject)
        {
            foreach (var item in targetObject.GetType().GetProperties())
            {
                var propertySetMethod = item.GetSetMethod();
                var value = item.GetGetMethod().Invoke(sourceObject, null);
                propertySetMethod.Invoke(targetObject, new object[] { value });
            }
        }

        public static string GetTextResource(string key)
        {
            return (string)(System.Windows.Application.Current.TryFindResource(key) ?? "");
        }
    }
}