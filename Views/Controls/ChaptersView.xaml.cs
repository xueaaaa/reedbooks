using ReedBooks.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReedBooks.Views.Controls
{
    public partial class ChaptersView : TreeView
    {
        public static readonly DependencyProperty NavigationProperty =
            DependencyProperty.Register("Navigation", typeof(List<NavigationItem>), typeof(ChaptersView));
        public static readonly DependencyProperty SelectedItemChangedProperty = 
            DependencyProperty.Register("OnSelectedItemChanged", typeof(RelayCommand), typeof(ChaptersView));

        public List<NavigationItem> Navigation
        {
            get => (List<NavigationItem>)GetValue(NavigationProperty);
            set => SetValue(NavigationProperty, value);
        }

        public new RelayCommand OnSelectedItemChanged
        {
            get => (RelayCommand)GetValue(SelectedItemChangedProperty);
            set => SetValue(SelectedItemChangedProperty, value);
        }

        public NavigationItem ViewSelectedItem
        {
            get => Control.SelectedItem as NavigationItem;
            set
            {
                int index = Control.Items.IndexOf(value);
                if (index != -1)
                {
                    var item = Control.Items[index];
                    (item as TreeViewItem).IsSelected = true;
                }
            }
        }

        public ChaptersView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object param = Control.SelectedItem as NavigationItem;
            OnSelectedItemChanged.Execute(param);
        }
    }
}
