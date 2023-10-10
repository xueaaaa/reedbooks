using ReedBooks.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SearchCommand.Execute(((TextBox)sender).Text);
        }
    }
}
