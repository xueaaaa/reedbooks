using ReedBooks.ViewModels;
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
    }
}
