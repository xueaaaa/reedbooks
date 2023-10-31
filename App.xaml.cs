using Microsoft.EntityFrameworkCore;
using ReedBooks.Core;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using AppContext = ReedBooks.Core.AppContext;

namespace ReedBooks
{
    public partial class App : Application
    {
        public const string LIGHT_THEME_NAME = "theme_light";
        public const string DARK_THEME_NAME = "theme_dark";
        public const string COVERS_DIRECTORY = "\\covers\\";
        public const string EPUBS_DIRECTORY = "\\epubs\\";

        public static AppContext ApplicationContext { get; private set; }
        
        public App()
        {
            ApplicationContext = new AppContext();
            ApplicationContext.Database.EnsureCreated();
            //ApplicationContext.Books.RemoveRange(ApplicationContext.Books.ToList());
            //ApplicationContext.SaveChanges();
            ApplicationContext.Books.Load();

            EnsureCreated();

            Localizator.AddLang(new CultureInfo("ru"));
            Localizator.AddLang(new CultureInfo("en"));
            Localizator.LanguageChanged += App_LanguageChanged;
        }

        public void ChangeTheme(string themeName)
        {
            var old = Current.Resources.MergedDictionaries.Where(a => a.Source.OriginalString.EndsWith("theme.xaml")).First();
            Current.Resources.MergedDictionaries.Remove(old);
            ResourceDictionary ne = new ResourceDictionary();
            ne.Source = new Uri($"Resources/Themes/{themeName}.theme.xaml", UriKind.Relative);
            Current.Resources.MergedDictionaries.Add(ne);
        }

        public void EnsureCreated()
        {
            var coversPath = $"{Directory.GetCurrentDirectory()}{COVERS_DIRECTORY}";
            var epubsPath = $"{Directory.GetCurrentDirectory()}{EPUBS_DIRECTORY}";

            if (!Directory.Exists(coversPath))
                Directory.CreateDirectory(coversPath);
            if (!Directory.Exists(epubsPath))
                Directory.CreateDirectory(epubsPath);
        }

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            ReedBooks.Properties.Settings.Default.Language = Localizator.CurrentLanguage;
            ReedBooks.Properties.Settings.Default.Save();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var lang = ReedBooks.Properties.Settings.Default.Language;
            Localizator.CurrentLanguage = lang == null ?
                new CultureInfo("ru") :
                lang;

            var theme = ReedBooks.Properties.Settings.Default.Theme;
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
    }
}
