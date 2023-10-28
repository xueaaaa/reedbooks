﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ReedBooks.Core
{
    public static class Localizator
    {
        /// <summary>
        /// Called when the application language is changed
        /// </summary>
        public static event EventHandler LanguageChanged;

        private static List<CultureInfo> _appLanguages = new List<CultureInfo>();
        /// <summary>
        /// List of all available application languages in CultureInfo format 
        /// </summary>
        public static List<CultureInfo> AppLanguages
        {
            get { return _appLanguages; }
        }

        /// <summary>
        /// Current application language
        /// </summary>
        public static CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value == null) throw new ArgumentNullException("cannot use null language");
                if (value == Thread.CurrentThread.CurrentUICulture) return;

                Thread.CurrentThread.CurrentUICulture = value;
                ResourceDictionary dictionary = new ResourceDictionary();
                switch (value.Name)
                {
                    default:
                        dictionary.Source = new Uri("Resources/Locales/lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.Where(d =>
                d.Source != null && d.Source.OriginalString.StartsWith("/Resources/Locales/lang.")).First();

                if (oldDictionary != null)
                {
                    int i = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Insert(i, dictionary);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a new language to the list of available application languages
        /// </summary>
        /// <param name="culture">Language</param>
        public static void AddLang(CultureInfo culture)
        {
            _appLanguages.Add(culture);
        }
    }
}
