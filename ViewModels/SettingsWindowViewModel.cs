using ReedBooks.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

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
                        DisplayName = lang.NativeName,
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
                    DisplayName = Application.Current.Resources["theme_light"].ToString(),
                    Tag = "theme_light"
                };
                themes.Add(lightTheme);

                var darkTheme = new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources["theme_dark"].ToString(),
                    Tag = "theme_dark"
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

        public SettingsWindowViewModel()
        {
        }
    }
}
