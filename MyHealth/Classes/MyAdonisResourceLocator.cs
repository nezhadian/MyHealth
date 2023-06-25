using AdonisUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyHealth
{
    class MyAdonisResourceLocator
    {
        static List<ResourceDictionary> AdonisColorSchemeList ;

        public static void DetectAllAdonisColorSchemes(ResourceDictionary root,Uri[] targets)
        {
            AdonisColorSchemeList = new List<ResourceDictionary>();
            foreach (var item in root.MergedDictionaries)
            {
                if (targets.Any((i) => item.Source == i))
                {
                    AdonisColorSchemeList.Add(item);
                }

            }
        }

        internal static void SetColorScheme(Uri colorSchemeUri)
        {
            foreach (var item in AdonisColorSchemeList)
            {
                item.Source = colorSchemeUri;
            }
        }

        static MyAdonisResourceLocator()
        {
            DetectAllAdonisColorSchemes(Application.Current.Resources, new Uri[] { ResourceLocator.DarkColorScheme, ResourceLocator.LightColorScheme });
        }
    }
}
