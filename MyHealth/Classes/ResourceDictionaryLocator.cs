using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyHealth
{
    class ResourceDictionaryLocator
    {
        ResourceDictionary resourceDictionary;
        string targetDirectory;

        public virtual void ChangeFile(string fileName)
        {
            resourceDictionary.Source = new Uri(Path.Combine(targetDirectory,fileName),UriKind.Relative);
        }

        public ResourceDictionaryLocator(string defaultFilePath)
        {
            targetDirectory = Path.GetDirectoryName(defaultFilePath);
            resourceDictionary = Application.Current.Resources.MergedDictionaries.First((rs) => rs.Source != null && rs.Source.OriginalString == defaultFilePath);
        }

        protected ResourceDictionaryLocator() { }
    }
}
