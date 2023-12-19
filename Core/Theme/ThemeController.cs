using Ionic.Zip;
using ReedBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace ReedBooks.Core.Theme
{
    public class ThemeController : ObservableObject
    {
        public static readonly string THEMES_PATH = $"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\";
        public static readonly string[] STANDART_THEME_NAMES = { "light", "dark" };

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
                if (Path.GetExtension(item) != ".rbtheme") continue;

                Theme theme = Open(item);
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

            if(STANDART_THEME_NAMES.Contains(themeName))
                ne.Source = new Uri($"Resources\\Themes\\{themeName}.theme.xaml", UriKind.Relative);
            else
                ne.Source = new Uri($"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\{themeName}.theme.xaml", UriKind.Absolute);

            Application.Current.Resources.MergedDictionaries.Add(ne);
        }

        public static Theme Open(string path)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                Theme theme = new Theme();

                ZipEntry e = zip["info.json"];
                string tempPath = $"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\temp\\";
                e.Extract(tempPath, ExtractExistingFileAction.OverwriteSilently);

                using (StreamReader stream = new StreamReader($"{tempPath}\\{e.FileName}"))
                {
                    theme = JsonSerializer.Deserialize<Theme>(stream.ReadToEnd());

                    ZipEntry e1 = zip[$"{theme.Name}.theme.xaml"];
                    string xamlPath = $"{Directory.GetCurrentDirectory()}\\Resources\\Themes\\";
                    if(!File.Exists($"{xamlPath}\\{e1.FileName}"))
                        e1.Extract(xamlPath, ExtractExistingFileAction.OverwriteSilently);

                    theme.FilePath = $"{xamlPath}\\{e1.FileName}";
                }

                File.Delete($"{tempPath}\\{e.FileName}");
                return theme;
            }
        }

        public ObservableCollection<SettingsParameterViewModel> LoadStandart()
        {
            var themes = new ObservableCollection<SettingsParameterViewModel>();

            foreach (var item in STANDART_THEME_NAMES)
            {
                var theme = new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources[item].ToString(),
                    Tag = item
                };

                themes.Add(theme);
            }

            return themes;
        }
    }
}
