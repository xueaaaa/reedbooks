using System.Collections.Generic;
using System.IO;

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
    }
}
