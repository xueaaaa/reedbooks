using System.Linq;
using VersOne.Epub;
using System.Windows.Documents;
using TheArtOfDev.HtmlRenderer.WPF;
using System.Windows;
using System.Windows.Media;
using ReedBooks.Views.Controls;
using System.Collections.Generic;

namespace ReedBooks.Models.Book
{
    public static class BookContentLoader
    {
        public static FlowDocument LoadChapterFromEpub(EpubBook book, string contentFilePath)
        {
            var content = book.ReadingOrder.Where(c => c.FilePath == contentFilePath).First();
            var html = new HtmlPanel();
            html.Text = content.Content;

            var background = (Color)ColorConverter.ConvertFromString(Application.Current.Resources["container_background_color"].ToString());
            html.Background = new SolidColorBrush(background);
            var foreground = (Color)ColorConverter.ConvertFromString(Application.Current.Resources["text_color"].ToString());
            html.Foreground = new SolidColorBrush(foreground);

            html.PreviewMouseDown += (sender, e) => e.Handled = true;
            html.IsSelectionEnabled = true;

            var uiContainer = new BlockUIContainer(html);
            var flowDoc = new FlowDocument();
            flowDoc.Blocks.Add(uiContainer);

            return flowDoc;
        }

        public static List<NavigationItem> LoadNavigation(EpubBook book)
        {
            var navigaion = new List<NavigationItem>();

            foreach (var item in book.Navigation)
            {
                var navItem = LoadNavigationItem(item);
                navigaion.Add(navItem);
            }

            return navigaion;
        }

        private static NavigationItem LoadNavigationItem(EpubNavigationItem item)
        {
            var navigationItem = new NavigationItem();
            navigationItem.Header = item.Title;
            navigationItem.Link = item.Link.ContentFilePath;

            if (item.NestedItems.Count > 0)
            {
                List<NavigationItem> nestedItems = new List<NavigationItem>();

                foreach (var nestedItem in item.NestedItems)
                {
                    var i = LoadNavigationItem(nestedItem);
                    nestedItems.Add(i);
                }

                navigationItem.ItemsSource = nestedItems;
            }

            return navigationItem;
        }
    }
}
