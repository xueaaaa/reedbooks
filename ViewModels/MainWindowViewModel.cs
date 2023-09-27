using ReedBooks.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Visibility _sidePanelVisibility;
        public Visibility SidePanelVisibility
        {
            get { return _sidePanelVisibility; }
            set
            {
                if (_sidePanelVisibility != value)
                {
                    _sidePanelVisibility = value;
                    OnPropertyChanged(nameof(SidePanelVisibility));
                }
            }
        }
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value >= 0) _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        public ICommand ChangeSidePanelVisibilityCommand { get; }
        public ICommand SwitchToTabCommand { get; }

        public MainWindowViewModel()
        {
            ChangeSidePanelVisibilityCommand = new RelayCommand(obj => ChangeSidePanelVisibility());
            SwitchToTabCommand = new RelayCommand(obj => SwitchToTab(obj));
        }

        private void ChangeSidePanelVisibility()
        {
            switch (SidePanelVisibility)
            {
                case Visibility.Visible:
                    SidePanelVisibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                    SidePanelVisibility = Visibility.Visible;
                    break;
                default:
                    SidePanelVisibility = Visibility.Visible;
                    break;
            }
        }

        private void SwitchToTab(object param)
        {
            SelectedTab = Convert.ToInt32(param);
        }
    }
}
