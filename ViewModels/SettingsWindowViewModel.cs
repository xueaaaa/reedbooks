using Microsoft.Win32;
using ReedBooks.Core;
using ReedBooks.Core.Theme;
using ReedBooks.Views;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Version = ReedBooks.Core.Version.Version;

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
                return _themes;
            }
            set 
            {
                _themes = value;
                OnPropertyChanged(nameof(Themes));
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

        private bool _showInteractionHints;
        public bool ShowInteractionHints
        {
            get => _showInteractionHints;
            set
            {
                _showInteractionHints = value;
                OnPropertyChanged(nameof(ShowInteractionHints));
            }
        }

        private ushort _selectedCurrentCountedDays;
        public ushort SelectedCurrentCountedDays
        {
            get => _selectedCurrentCountedDays;
            set
            {
                _selectedCurrentCountedDays = value;
                OnPropertyChanged(nameof(SelectedCurrentCountedDays));
            }
        }

        private byte _recentBooksNumberDisplaying;
        public byte RecentBookNumberDisplaying
        {
            get => _recentBooksNumberDisplaying;
            set
            {
                _recentBooksNumberDisplaying = value;
                OnPropertyChanged(nameof(RecentBookNumberDisplaying));
            }
        }

        private ushort _timeGoal;
        public ushort TimeGoal
        {
            get => _timeGoal;
            set
            {
                _timeGoal = value;
                OnPropertyChanged(nameof(TimeGoal));
            }
        }

        private bool _hideReadingNow;
        public bool HideReadingNow
        {
            get => _hideReadingNow;
            set
            {
                _hideReadingNow = value;
                OnPropertyChanged(nameof(HideReadingNow));
            }
        }

        private ObservableCollection<SettingsParameterViewModel> _tabs;
        public ObservableCollection<SettingsParameterViewModel> Tabs
        {
            get
            {
                return _tabs;
            }
            set
            {
                _tabs = value;
                OnPropertyChanged(nameof(Tabs));
            }
        }

        private SettingsParameterViewModel _selectedTab;
        public SettingsParameterViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        private bool _deleteUnusedAutomatically;
        public bool DeleteUnusedAutomatically
        {
            get => _deleteUnusedAutomatically;
            set
            {
                _deleteUnusedAutomatically = value;
                OnPropertyChanged(nameof(DeleteUnusedAutomatically));
            }
        }

        private bool _updateAutomatically;
        public bool UpdateAutomatically
        {
            get => _updateAutomatically;
            set
            {
                _updateAutomatically = value;
                OnPropertyChanged(nameof(UpdateAutomatically));
            }
        }

        private bool _deleteThemeEnabled;
        public bool DeleteThemeEnabled
        {
            get => _deleteThemeEnabled;
            set
            {
                _deleteThemeEnabled = value;
                OnPropertyChanged(nameof(DeleteThemeEnabled));
            }
        }

        public Version InstalledVersion
        {
            get => Version.Local;
        }

        public string SummarySizeString
        {
            get => $"~{Math.Round(App.StorageManager.SummarySize, 1)} {Application.Current.Resources["megabytes"]}";
        }

        public string DeleteUnusedFilesHint
        {
            get => $"{Application.Current.Resources["s_delete_unused_files_hint"]} {Math.Round(App.StorageManager.UnusedFilesSize, 1)} {Application.Current.Resources["megabytes"]}";
        }

        public ICommand LoadThemeCommand { get; }
        public ICommand DeleteThemeCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteUnusedFilesCommand { get; }
        public ICommand DeleteAllBooksCommand { get; }

        public SettingsWindowViewModel()
        {
            LoadThemeCommand = new RelayCommand(obj => LoadTheme());
            DeleteThemeCommand = new RelayCommand(obj => DeleteTheme());
            SaveCommand = new RelayCommand(obj => Save());
            DeleteUnusedFilesCommand = new RelayCommand(obj => DeleteUnusedFiles());
            DeleteAllBooksCommand = new RelayCommand(obj => DeleteAllBooks());

            Themes = App.ThemeController.Load();
            App.ThemeController.OnLoadedThemesChanged += () => {
                Themes = App.ThemeController.Load();
                SelectedTheme = Themes.Where(t => t.Tag == Properties.Settings.Default.Theme).First();
            };
            App.ThemeController.OnThemeChanged += (name) => {
                if (ThemeController.STANDART_THEME_NAMES.Contains(name)) DeleteThemeEnabled = false;
                else DeleteThemeEnabled = true;
            };
            if (ThemeController.STANDART_THEME_NAMES.Contains(Properties.Settings.Default.Theme)) DeleteThemeEnabled = false;
            else DeleteThemeEnabled = true;

            SelectedLanguage = Languages.Where(l => l.Tag == Properties.Settings.Default.Language.Name).First();
            SelectedTheme = Themes.Where(t => t.Tag == Properties.Settings.Default.Theme).First();
            SelectedCurrentCountedDays = Properties.Settings.Default.CurrentCountedDays;
            RecentBookNumberDisplaying = Properties.Settings.Default.RecentBooksNumberDisplaying;
            TimeGoal = Properties.Settings.Default.TimeGoal;
            ShowInteractionHints = Properties.Settings.Default.ShowInteractionHints;
            HideReadingNow = Properties.Settings.Default.HideReadingNow;
            UpdateAutomatically = Properties.Settings.Default.UpdateAutomatically;
            DeleteUnusedAutomatically = Properties.Settings.Default.DeleteUnusedAutomatically;
            ReloadTabs();
        }

        public void LoadTheme()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "ReedBooks Theme (.rbtheme)|*.rbtheme";

            if (ofd.ShowDialog() == true)
            {
                var filePath = ofd.FileName;
                App.ThemeController.AddNew(filePath);
            }
        }

        public void DeleteTheme()
        {
            if (new DialogWindow(App.Current.Resources["dialog_delete_theme_title"].ToString(),
                Application.Current.Resources["dialog_delete_theme_content"].ToString()).ShowDialog() == true)
                App.ThemeController.Remove(Properties.Settings.Default.Theme);
        }

        public void Save()
        {
            if (_selectedLanguage != null) 
            {
                var info = new CultureInfo(_selectedLanguage.Tag);
                Properties.Settings.Default.Language = info;
                Localizator.CurrentLanguage = info;
            }
            if (_selectedTheme != null) 
            { 
                Properties.Settings.Default.Theme = _selectedTheme.Tag;
                App.ThemeController.ChangeTheme(_selectedTheme.Tag);
            }
            if (_selectedCurrentCountedDays != 0) Properties.Settings.Default.CurrentCountedDays = _selectedCurrentCountedDays;
            if (_recentBooksNumberDisplaying != 0) Properties.Settings.Default.RecentBooksNumberDisplaying = _recentBooksNumberDisplaying;
            if (_selectedTab != null) Properties.Settings.Default.DefaultTab = Convert.ToByte(_selectedTab.Tag);
            Properties.Settings.Default.ShowInteractionHints = ShowInteractionHints;
            Properties.Settings.Default.UpdateAutomatically = UpdateAutomatically;
            Properties.Settings.Default.DeleteUnusedAutomatically = DeleteUnusedAutomatically;
            Properties.Settings.Default.HideReadingNow = HideReadingNow;

            Themes = App.ThemeController.Load();
            SelectedTheme = Themes.Where(t => t.Tag == Properties.Settings.Default.Theme).First();

            //For apply language settings to these menu labels
            ReloadTabs();
            OnPropertyChanged(nameof(SummarySizeString));
            OnPropertyChanged(nameof(DeleteUnusedFilesHint));

            Properties.Settings.Default.Save();
        }

        public void DeleteUnusedFiles()
        {
            App.StorageManager.DeleteUnusedFiles();
            OnPropertyChanged(nameof(DeleteUnusedFilesHint));
            OnPropertyChanged(nameof(SummarySizeString));
        }

        public void DeleteAllBooks()
        {
            var dW = new DialogWindow(Application.Current.Resources["dialog_delete_all_books_title"].ToString(),
                Application.Current.Resources["dialog_delete_all_books_content"].ToString());

            if (dW.ShowDialog() == true)
            {
                App.StorageManager.DeleteAllBooks();
                App.Restart();
            }
        }

        private void ReloadTabs()
        {
            Tabs = new ObservableCollection<SettingsParameterViewModel>
            {
                new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources["m_reading_now"].ToString(),
                    Tag = "0"
                },
                new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources["m_all_books"].ToString(),
                    Tag = "1"
                },
                new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources["m_search"].ToString(),
                    Tag = "2"
                },
                new SettingsParameterViewModel
                {
                    DisplayName = Application.Current.Resources["m_books_shop"].ToString(),
                    Tag = "3"
                }
            };

            SelectedTab = Tabs.Where(t => t.Tag == Convert.ToString(Properties.Settings.Default.DefaultTab)).First();
        }
    }
}
