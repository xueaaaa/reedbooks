using ReedBooks.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            ((AuthorizationWindowViewModel)DataContext).Success += () =>
            {
                DialogResult = true;
            };
            ((AuthorizationWindowViewModel)DataContext).Reset += () =>
            {
                DialogResult = null;
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                int digit = e.Key - Key.D0;
                ((AuthorizationWindowViewModel)DataContext).EnterSymbol(Convert.ToInt32(digit));
            }
            else if(e.Key == Key.Back) ((AuthorizationWindowViewModel)DataContext).EraseSymbol();
        }
    }
}
