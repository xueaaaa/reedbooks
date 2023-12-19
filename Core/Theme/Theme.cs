using System.Text.Json.Serialization;

namespace ReedBooks.Core.Theme
{
    public class Theme : ObservableObject
    {
        private string _name;
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
