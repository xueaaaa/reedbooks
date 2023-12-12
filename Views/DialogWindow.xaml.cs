using ReedBooks.ViewModels;
using System.Windows;

namespace ReedBooks.Views
{
    public partial class DialogWindow : Window
    {
        public DialogWindow(string title, string content, Visibility showCancel = Visibility.Visible)
        {
            InitializeComponent();
            DataContext = new DialogWindowViewModel(title, content, showCancel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
