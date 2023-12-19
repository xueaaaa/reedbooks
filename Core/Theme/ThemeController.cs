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
        public static readonly string THEMES_DIRECTORY = $"{Directory.GetCurrentDirectory()}\\resources\\themes\\";
        public static readonly string[] STANDART_THEME_NAMES = { "light", "dark" };

        public delegate void LoadedThemesChangedEventHandler();
        public event LoadedThemesChangedEventHandler OnLoadedThemesChanged;

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

            foreach (var item in Directory.GetFiles(THEMES_DIRECTORY))
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
                ne.Source = new Uri($"{THEMES_DIRECTORY}\\{themeName}.theme.xaml", UriKind.Absolute);

            Application.Current.Resources.MergedDictionaries.Add(ne);
        }

        public void AddNew(string originPath)
        {
            string newPath = $"{THEMES_DIRECTORY}{Path.GetFileName(originPath)}";
            File.Move(originPath, newPath);

            Theme theme = Open(newPath);
            LoadedThemes.Add(theme);
            OnLoadedThemesChanged?.Invoke();
        }

        public static Theme Open(string path)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                Theme theme = new Theme();

                ZipEntry e = zip["info.json"];
                string tempPath = $"{THEMES_DIRECTORY}\\temp\\";
                e.Extract(tempPath, ExtractExistingFileAction.OverwriteSilently);

                using (StreamReader stream = new StreamReader($"{tempPath}\\{e.FileName}"))
                {
                    theme = JsonSerializer.Deserialize<Theme>(stream.ReadToEnd());

                    ZipEntry e1 = zip[$"{theme.Name}.theme.xaml"];
                    string xamlPath = $"{THEMES_DIRECTORY}";
                    if(!File.Exists($"{xamlPath}\\{e1.FileName}"))
                        e1.Extract(xamlPath, ExtractExistingFileAction.OverwriteSilently);

                    theme.FilePath = $"{xamlPath}\\{e1.FileName}";
                }

                File.Delete($"{tempPath}\\{e.FileName}");
                return theme;
            }
        }

        public ObservableCollection<SettingsParameterViewModel> Load()
        {
            var standart = LoadStandart();
            var custom = LoadCustom();

            return new ObservableCollection<SettingsParameterViewModel>(standart.Concat(custom));
        }

        private ObservableCollection<SettingsParameterViewModel> LoadCustom()
        {
            var themes = new ObservableCollection<SettingsParameterViewModel>();

            foreach (var theme in LoadedThemes)
            {
                var sTheme = new SettingsParameterViewModel
                {
                    DisplayName = $"{theme.DisplayName} ({theme.Author})",
                    Tag = theme.Name
                };

                themes.Add(sTheme);
            }

            return themes;
        }

        private ObservableCollection<SettingsParameterViewModel> LoadStandart()
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
