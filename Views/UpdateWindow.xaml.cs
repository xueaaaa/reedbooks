using ReedBooks.Core.Version;
using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
            DataContext = new UpdateWindowViewModel();
        }
    }
}
