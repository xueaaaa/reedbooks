using System.Windows.Documents;
using TheArtOfDev.HtmlRenderer.WPF;
using System.Windows;
using VersOne.Epub;

namespace ReedBooks.Core
{
    public class ContentLoader
    {
        public EpubBook Book { get; private set; }

        public ContentLoader(EpubBook book) {  Book = book; }

        public void Load(ref FlowDocument flowDocument)
        {
            foreach (var item in Book.ReadingOrder)
            {
                var htmlPanel = new HtmlPanel();
                htmlPanel.Text = item.Content;
                var uiContainer = new BlockUIContainer(htmlPanel);
                uiContainer.TextAlignment = TextAlignment.Justify;
                flowDocument.Blocks.Add(uiContainer);
            }            
        }
    }
}
