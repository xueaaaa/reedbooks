using ReedBooks.Core;
using ReedBooks.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        private ObservableCollection<SettingsParameterViewModel> _languages;
        public ObservableCollection<SettingsParameterViewModel> Languages
        {
            get
            {
                if (_languages == null)
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

                    _languages = items;
                }

                return _languages;
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

        private ObservableCollection<SettingsParameterViewModel> _themes;
        public ObservableCollection<SettingsParameterViewModel> Themes
        {
            get
            {
                if (_themes == null)
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

                    _themes = themes;
                }
                
                return _themes;
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

            SelectedLanguage = Languages.Where(l => l.Tag == Properties.Settings.Default.Language.Name).First();
            SelectedTheme = Themes.Where(t => t.Tag == Properties.Settings.Default.Theme).First();
        }

        public void Save()
        {
            if (_selectedLanguage != null) Properties.Settings.Default.Language = new CultureInfo(_selectedLanguage.Tag);
            if (_selectedTheme != null) Properties.Settings.Default.Theme = _selectedTheme.Tag;

            Properties.Settings.Default.Save();

            var dW = new DialogWindow(Application.Current.Resources["dialog_settings_title"].ToString(),
                Application.Current.Resources["dialog_settings_content"].ToString());

            if (dW.ShowDialog() == true)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
    }
}
