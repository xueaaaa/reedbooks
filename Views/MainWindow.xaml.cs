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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).ReadCommand.Execute(((ListView)sender).SelectedItem);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            ((MainWindowViewModel)DataContext).HandleFileDropCommand.Execute(e);
        }
    }
}
