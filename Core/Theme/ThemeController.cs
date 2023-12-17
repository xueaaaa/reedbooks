using System;
using System.Collections.Generic;
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

        public void ChangeTheme(string themeName)
        {
            var old = Application.Current.Resources.MergedDictionaries.Where(a => a.Source.OriginalString.EndsWith("theme.xaml")).First();
            Application.Current.Resources.MergedDictionaries.Remove(old);
            ResourceDictionary ne = new ResourceDictionary();
            ne.Source = new Uri($"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\{themeName}.theme.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(ne);
        }
    }
}
