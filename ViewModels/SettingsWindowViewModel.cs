using ReedBooks.Core;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        public ObservableCollection<SettingsParameterViewModel> Languages
        {
            get
            {
                var items = new ObservableCollection<SettingsParameterViewModel>();
                foreach (var lang in Localizator.AppLanguages)
                {
                    var item = new SettingsParameterViewModel
                    {
                        DisplayName = char.ToUpper(lang.NativeName[0]) + lang.NativeName.Substring(1),
                        Tag = lang.Name
                    };

                    items.Add(item);
                }

                return items;
            }
        }

        private SettingsParameterViewModel _selectedLanguage;
        public SettingsParameterViewModel SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        public ObservableCollection<SettingsParameterViewModel> Themes
        {
            get
            {
                var themes = new ObservableCollection<SettingsParameterViewModel>();

                var lightTheme = new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources[App.LIGHT_THEME_NAME].ToString(),
                    Tag = App.LIGHT_THEME_NAME
                };
                themes.Add(lightTheme);

                var darkTheme = new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources[App.DARK_THEME_NAME].ToString(),
                    Tag = App.DARK_THEME_NAME
                };
                themes.Add(darkTheme);
                    
                return themes;
            }
        }

        private SettingsParameterViewModel _selectedTheme;
        public SettingsParameterViewModel SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        public ICommand SaveCommand { get; }

        public SettingsWindowViewModel()
        {
            SaveCommand = new RelayCommand(obj => Save());
        }

        public void Save()
        {
            if (_selectedLanguage != null) Properties.Settings.Default.Language = new CultureInfo(_selectedLanguage.Tag);
            if (_selectedTheme != null) Properties.Settings.Default.Theme = _selectedTheme.Tag;

            Properties.Settings.Default.Save();
        }
    }
}
