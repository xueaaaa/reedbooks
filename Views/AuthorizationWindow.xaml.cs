using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            DataContext = new AuthorizationWindowViewModel();

            ((AuthorizationWindowViewModel)DataContext).Success += () =>
            {
                DialogResult = true;
            };
        }
    }
}
