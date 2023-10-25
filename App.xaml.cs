using Microsoft.EntityFrameworkCore;
using ReedBooks.Core;
using System;
using System.Globalization;
using System.Windows;
using AppContext = ReedBooks.Core.AppContext;

namespace ReedBooks
{
    public partial class App : Application
    {
        public static AppContext ApplicationContext { get; private set; }
        
        public App()
        {
            ApplicationContext = new AppContext();
            ApplicationContext.Database.EnsureCreated();
            ApplicationContext.Books.Load();

            Localizator.AddLang(new CultureInfo("ru_RU"));
            Localizator.LanguageChanged += App_LanguageChanged;
        }
        

        private void App_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Localizator.CurrentLanguage = ReedBooks.Properties.Settings.Default.DefaultLanguage;
        }
        private void App_LanguageChanged(object sender, EventArgs e)
        {
            ReedBooks.Properties.Settings.Default.DefaultLanguage = Localizator.CurrentLanguage;
            ReedBooks.Properties.Settings.Default.Save();
        }
    }
}
