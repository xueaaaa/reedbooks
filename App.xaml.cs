using Microsoft.EntityFrameworkCore;
using ReedBooks.Core;
using ReedBooks.Properties;
using ReedBooks.Views;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using AppContext = ReedBooks.Core.AppContext;

namespace ReedBooks
{
    public partial class App : Application
    {
        public const string LIGHT_THEME_NAME = "theme_light";
        public const string DARK_THEME_NAME = "theme_dark";

        public static AppContext ApplicationContext { get; private set; }
        public static StorageManager StorageManager { get; private set; }
        
        public App()
        {
            ApplicationContext = new AppContext();
            ApplicationContext.Database.EnsureCreated();
            ApplicationContext.Books.Load();
            ApplicationContext.Quotes.Load();

            StorageManager = new StorageManager();

            StorageManager.EnsureCreated();

            Localizator.AddLang(new CultureInfo("ru"));
            Localizator.AddLang(new CultureInfo("en"));
            Localizator.AddLang(new CultureInfo("es-ES"));
            Localizator.AddLang(new CultureInfo("de"));
            Localizator.LanguageChanged += App_LanguageChanged;
        }

        /// <summary>
        /// Changes the color theme of the application
        /// </summary>
        /// <param name="themeName">Theme name ("light" or "dark")</param>
        public void ChangeTheme(string themeName)
        {
            var old = Current.Resources.MergedDictionaries.Where(a => a.Source.OriginalString.EndsWith("theme.xaml")).First();
            Current.Resources.MergedDictionaries.Remove(old);
            ResourceDictionary ne = new ResourceDictionary();
            ne.Source = new Uri($"Resources/Themes/{themeName}.theme.xaml", UriKind.Relative);
            Current.Resources.MergedDictionaries.Add(ne);
        }

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            Settings.Default.Language = Localizator.CurrentLanguage;
            Settings.Default.Save();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Gets the set language from settings and checks it for null
            // If the received language is null, the value of the default language is assigned
            var lang = Settings.Default.Language;
            Localizator.CurrentLanguage = lang == null ?
                Settings.Default.DefaultLanguage :
                lang;

            var theme = Settings.Default.Theme;
            switch (theme)
            {
                case LIGHT_THEME_NAME:
                    ChangeTheme("light");
                    break;
                case DARK_THEME_NAME:
                    ChangeTheme("dark");
                    break;
                default:
                    ChangeTheme("light");
                    break;
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var dialog = new DialogWindow(Current.Resources["dialog_error_title"].ToString(),
                Current.Resources["dialog_fatal_error_content"].ToString() + ' ' + e.Exception.Message);
            dialog.ShowDialog();
            e.Handled = true;
        }
    }
}
