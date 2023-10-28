using ReedBooks.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ReedBooks.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        public ObservableCollection<LanguageViewModel> Languages
        {
            get
            {
                var items = new ObservableCollection<LanguageViewModel>();
                foreach (var lang in Localizator.AppLanguages)
                {
                    var item = new LanguageViewModel
                    {
                        DisplayName = lang.NativeName,
                        Tag = lang.Name
                    };

                    items.Add(item);
                }

                return items;
            }
        }

        private LanguageViewModel _selectedLanguage;
        public LanguageViewModel SelectedLanguage
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

        public ObservableCollection<string> Themes
        {
            get
            {
                var themes = new ObservableCollection<string>();
                themes.Add(Application.Current.Resources["theme_light"].ToString());
                themes.Add(Application.Current.Resources["theme_dark"].ToString());
                    
                return themes;
            }
        }

        private string _selectedTheme;
        public string SelectedTheme
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
