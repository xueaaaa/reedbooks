namespace ReedBooks.Core.Theme
{
    public class Theme : ObservableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public Theme(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }
    } 
}
