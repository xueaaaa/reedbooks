using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ReedBooks
{
    public partial class App : Application
    {
        public static event EventHandler LanguageChanged;

        private static List<CultureInfo> _appLanguages = new List<CultureInfo>();
        public static List<CultureInfo> AppLanguages
        {
            get { return _appLanguages; }
        }

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

                ResourceDictionary oldDictionary = Current.Resources.MergedDictionaries.Where(d => 
                d.Source != null && d.Source.OriginalString.StartsWith("/Resources/Locales/lang.")).First();

                if(oldDictionary != null)
                {
                    int i = Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                    Current.Resources.MergedDictionaries.Remove(oldDictionary);
                    Current.Resources.MergedDictionaries.Insert(i, dictionary);
                } else
                {
                    Current.Resources.MergedDictionaries.Add(dictionary);
                }

                LanguageChanged(Current, new EventArgs());
            }
        }

        public App()
        {
            _appLanguages.Clear();
            _appLanguages.Add(new CultureInfo("ru"));
            LanguageChanged += App_LanguageChanged;
        }
        

        private void App_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            CurrentLanguage = ReedBooks.Properties.Settings.Default.DefaultLanguage;
        }
        private void App_LanguageChanged(object sender, EventArgs e)
        {
            ReedBooks.Properties.Settings.Default.DefaultLanguage = CurrentLanguage;
            ReedBooks.Properties.Settings.Default.Save();
        }
    }
}
