using ReedBooks.Core;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ReedBooks.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        private ObservableCollection<CultureInfo> _appLanguages;
        public ObservableCollection<CultureInfo> AppLanguages
        {
            get => _appLanguages;
        }

        private CultureInfo _selectedLanguage;
        public object SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (value != null)
                {
                    _selectedLanguage = (CultureInfo)value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        public SettingsWindowViewModel()
        {
            _appLanguages = new ObservableCollection<CultureInfo>(App.AppLanguages);
            _selectedLanguage = App.CurrentLanguage;
        }
    }
}
