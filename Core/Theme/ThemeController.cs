using ReedBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace ReedBooks.Core.Theme
{
    public class ThemeController : ObservableObject
    {
        public static readonly string THEMES_PATH = $"{Directory.GetCurrentDirectory()}/Resources/Themes/";

        private List<Theme> _loadedThemes;
        public List<Theme> LoadedThemes
        {
            get => _loadedThemes;
            set
            {
                _loadedThemes = value;
                OnPropertyChanged(nameof(LoadedThemes));
            }
        }

        public ThemeController()
        {
            LoadedThemes = new List<Theme>();

            foreach (var item in Directory.GetFiles(THEMES_PATH))
            {
                Theme theme = new Theme("", item);
                LoadedThemes.Add(theme);
            }
        }

        /// <summary>
        /// Changes the color theme of the application
        /// </summary>
        /// <param name="themeName">Theme name</param>
        public void ChangeTheme(string themeName)
        {
            var old = Application.Current.Resources.MergedDictionaries.Where(a => a.Source != null && a.Source.OriginalString.EndsWith("theme.xaml")).First();
            Application.Current.Resources.MergedDictionaries.Remove(old);
            ResourceDictionary ne = new ResourceDictionary();
            ne.Source = new Uri($"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\{themeName}.theme.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(ne);
        }

        public ObservableCollection<SettingsParameterViewModel> LoadInternal()
        {
            var themes = new ObservableCollection<SettingsParameterViewModel>();

            var lightTheme = new SettingsParameterViewModel
            {
                DisplayName = Application.Current.Resources["light"].ToString(),
                Tag = "light"
            };
            themes.Add(lightTheme);

            var darkTheme = new SettingsParameterViewModel
            {
                DisplayName = Application.Current.Resources["dark"].ToString(),
                Tag = "dark"
            };
            themes.Add(darkTheme);

            return themes;
        }
    }
}
