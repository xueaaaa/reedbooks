using ReedBooks.Core;
using ReedBooks.Core.Theme;
using ReedBooks.Core.Version;
using ReedBooks.Properties;
using ReedBooks.Views;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using AppContext = ReedBooks.Core.AppContext;

namespace ReedBooks
{
    public partial class App : Application
    {
        public static AppContext ApplicationContext { get; private set; }
        public static StorageManager StorageManager { get; private set; }
        public static Updater Updater { get; private set; }
        public static ThemeController ThemeController { get; private set; }

        public App()
        {
            SplashScreen splashScreen = new SplashScreen("\\Resources\\splash.png");
            splashScreen.Show(true);

            StorageManager = new StorageManager();
            Updater = new Updater();
            ThemeController = new ThemeController();
            StorageManager.EnsureCreated();

            ApplicationContext = new AppContext();

            Localizator.AddLang(new CultureInfo("ru"));
            Localizator.AddLang(new CultureInfo("en"));
            Localizator.AddLang(new CultureInfo("es-ES"));
            Localizator.AddLang(new CultureInfo("de"));
            Localizator.LanguageChanged += App_LanguageChanged;
        }

        public static void Restart()
        {
            System.Diagnostics.Process.Start(ResourceAssembly.Location);
            Current.Shutdown();
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
            ThemeController.ChangeTheme(theme);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var dialog = new DialogWindow(Current.Resources["dialog_error_title"].ToString(),
                Current.Resources["dialog_fatal_error_content"].ToString() + ' ' + e.Exception.Message,
                Visibility.Hidden);
            dialog.ShowDialog();
            e.Handled = true;
        }
    }
}
