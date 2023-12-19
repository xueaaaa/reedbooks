using System.Text.Json.Serialization;

namespace ReedBooks.Core.Theme
{
    /// <summary>
    /// ReedBooks theme
    /// </summary>
    public class Theme : ObservableObject
    {
        private string _name;
        /// <summary>
        /// The internal name of the theme (must match the name of the resource file up to .theme.xaml)
        /// </summary>
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _displayName;
        /// <summary>
        /// Displayed theme title (in one language only)
        /// </summary>
        [JsonPropertyName("display")]
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private string _author;
        /// <summary>
        /// Author of the theme (indicated in brackets next to the theme title)
        /// </summary>
        [JsonPropertyName("author")]
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _filePath;
        /// <summary>
        /// Resource file path
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public Theme() { }

        public Theme(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }
    }
}
