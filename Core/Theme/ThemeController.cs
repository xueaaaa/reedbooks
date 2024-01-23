using Ionic.Zip;
using MaterialDesignThemes.Wpf;
using ReedBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace ReedBooks.Core.Theme
{
    public class ThemeController : ObservableObject
    {
        public static readonly string THEMES_DIRECTORY = $"{Directory.GetCurrentDirectory()}\\resources\\themes\\";
        public static readonly string[] STANDART_THEME_NAMES = { "light", "dark" };

        public delegate void LoadedThemesChangedEventHandler();
        public delegate void ThemeChanged(string newThemeName);
        public event LoadedThemesChangedEventHandler OnLoadedThemesChanged;
        public event ThemeChanged OnThemeChanged;

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

            if (!File.Exists($"{THEMES_DIRECTORY}\\{themeName}.theme.xaml") && !STANDART_THEME_NAMES.Contains(themeName))
            {
                ne.Source = new Uri($"Resources\\Themes\\light.theme.xaml", UriKind.Relative);
                Properties.Settings.Default.Theme = "light";
                Properties.Settings.Default.Save();
            }
            else if (STANDART_THEME_NAMES.Contains(themeName))
                ne.Source = new Uri($"Resources\\Themes\\{themeName}.theme.xaml", UriKind.Relative);
            else
                ne.Source = new Uri($"{THEMES_DIRECTORY}\\{themeName}.theme.xaml", UriKind.Absolute);

            Application.Current.Resources.MergedDictionaries.Add(ne);

            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            Color accent = (Color)ColorConverter.ConvertFromString(ne["accent_color"].ToString());
            Color hint = (Color)ColorConverter.ConvertFromString(ne["hint_color"].ToString());

            theme.SetPrimaryColor(accent);
            theme.SetSecondaryColor(hint);

            paletteHelper.SetTheme(theme);

            OnThemeChanged?.Invoke(themeName);
        }

        /// <summary>
        /// Downloads the ReedBooks theme file to the local theme folder and adds the theme to the list of downloaded themes.
        /// Notifies sub users on OnLoadedThemesChanged about adding a new theme
        /// </summary>
        /// <param name="originPath">Original file path</param>
        public void AddNew(string originPath)
        {
            string newPath = $"{THEMES_DIRECTORY}{Path.GetFileName(originPath)}";
            File.Move(originPath, newPath);

            Theme theme = Open(newPath);
            LoadedThemes.Add(theme);
            OnLoadedThemesChanged?.Invoke();
        }

        public void Remove(string name) 
        {
            string rbThemePath = $"{THEMES_DIRECTORY}{name}.rbtheme";
            string xamlPath = $"{THEMES_DIRECTORY}{name}.theme.xaml";

            File.Delete(rbThemePath);
            File.Delete(xamlPath);

            ChangeTheme(STANDART_THEME_NAMES[0]);
            OnLoadedThemesChanged?.Invoke();
        }

        /// <summary>
        /// Extracts the theme resources (if not unpacked) and returns an instance of the theme with the metadata specified in the theme file
        /// </summary>
        /// <param name="path">(Local) path to the theme file</param>
        /// <returns>An instance of the theme with the metadata specified in the theme file</returns>
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

        /// <summary>
        /// (For SettingsWindowViewModel only)
        /// Converts LoadedThemes from LoadedThemes to SettingsParameterViewModel
        /// </summary>
        internal ObservableCollection<SettingsParameterViewModel> Load()
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
