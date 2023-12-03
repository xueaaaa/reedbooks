using ReedBooks.Models.Book;
using ReedBooks.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class CollectionsView : ItemsControl
    {
        public CollectionsView()
        {
            InitializeComponent();
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).DeleteCollectionCommand.Execute(((Button)sender).CommandParameter);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SelectedCollectionBooks = new ObservableCollection<Book>();

            foreach (var item in ((ListBox)sender).SelectedItems)
            {
                ((MainWindowViewModel)DataContext).SelectedCollectionBooks.Add((Book)item);
            }
        }
    }
}
