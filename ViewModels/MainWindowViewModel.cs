using ReedBooks.Core;
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
        public ICommand ChangeSidePanelVisibilityCommand { get; }

        public MainWindowViewModel()
        {
            ChangeSidePanelVisibilityCommand = new RelayCommand(obj => ChangeSidePanelVisibility());
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
    }
}
