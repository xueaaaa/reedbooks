using ReedBooks.Core;
using System.Drawing;
using VersOne.Epub;

namespace ReedBooks.Models.Book
{
    public class Book : ObservableObject
    {
        public EpubBook Origin { get; }
        public Bitmap Cover { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int PagesCount { get; }
        public string Genre { get; set; }

        ~Book()
        {
            Cover.Dispose();
        }
    }
}
