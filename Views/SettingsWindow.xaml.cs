using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsWindowViewModel();
        }
    }
}
