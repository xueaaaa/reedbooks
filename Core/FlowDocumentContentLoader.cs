using System.Windows.Documents;
using TheArtOfDev.HtmlRenderer.WPF;
using VersOne.Epub;

namespace ReedBooks.Core
{
    public static class FlowDocumentContentLoader
    {
        public static FlowDocument LoadFromEpubBook(EpubBook epubBook)
        {
            var flowDocument = new FlowDocument();

            foreach (var item in epubBook.ReadingOrder)
            {
                var htmlPanel = new HtmlPanel();
                htmlPanel.Text = item.Content;
                var uiContainer = new BlockUIContainer(htmlPanel);
                flowDocument.Blocks.Add(uiContainer);
            }

            return flowDocument;
        }
    }
}
