using ReedBooks.Core;
using System.Collections.ObjectModel;

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
                        DisplayName = Localizator.GetLanguageName(lang),
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
            get { return _selectedLanguage; }
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        public SettingsWindowViewModel()
        {
        }
    }
}
