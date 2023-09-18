using ReedBooks.Core;
using ReedBooks.Models.Diary;
using System.Drawing;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VersOne.Epub;
using Path = System.IO.Path;

namespace ReedBooks.Models.Book
{
    public class Book : ObservableObject
    {
        [JsonIgnore] public EpubBook Origin { get; set; }
        [JsonIgnore] public Bitmap Cover { get; set; }

        private ReadingDiary _boundDiary;
        [JsonPropertyName("diary")] public ReadingDiary BoundDiary { get => _boundDiary; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("author")] public string Author { get; set; }
        [JsonPropertyName("chapters_count")] public int ChaptersCount { get; set; }
        [JsonPropertyName("genre")] public string Genre { get; set; }
        [JsonPropertyName("origin_link")] public string LinkToOrigin { get; set; }
        [JsonPropertyName("cover_link")] public string LinkToCover { get; set; }

        ~Book()
        {
            Cover.Dispose();
        }

        public async static Task<Book> Create(string path)
        {
            Book book = new Book();
            path = MoveToInternalFolder(path);

            EpubBook epubBook = await EpubReader.ReadBookAsync(path);
            book.Origin = epubBook;

            using (MemoryStream stream = new MemoryStream(epubBook.CoverImage))
            {
                Bitmap bitmap = new Bitmap(stream);
                book.Cover = bitmap;
                bitmap.Save($"{Directory.GetCurrentDirectory()}/covers/{Path.GetFileName(epubBook.FilePath)}.png");
            }

            book.Author = epubBook.Author;
            book.Name = epubBook.Title;
            book.ChaptersCount = epubBook.ReadingOrder.Count;

            return book;
        }

        private static string MoveToInternalFolder(string originPath)
        {
            string newPath = $"{Directory.GetCurrentDirectory()}/books/{Path.GetFileName(originPath)}.epub";
            File.Move(originPath, newPath);
            return newPath;
        }
    }
}
