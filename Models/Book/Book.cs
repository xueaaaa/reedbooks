using ReedBooks.Core;
using ReedBooks.Models.Diary;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VersOne.Epub;
using Path = System.IO.Path;

namespace ReedBooks.Models.Book
{
    public class Book : ObservableObject
    {
        [JsonPropertyName("guid")] public Guid Guid { get; set; }
        [JsonIgnore] public EpubBook Origin { get; set; }

        private ReadingDiary _boundDiary;
        [JsonPropertyName("diary")] public ReadingDiary BoundDiary { get => _boundDiary; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("author")] public string Author { get; set; }
        [JsonPropertyName("chapters_count")] public int ChaptersCount { get; set; }
        [JsonPropertyName("genre")] public string Genre { get; set; }
        [JsonPropertyName("origin_link")] public string LinkToOrigin { get; set; }
        [JsonPropertyName("cover_link")] public string LinkToCover { get; set; }

        /// <summary>
        /// Creates and returns an instance of a book from an external .epub file
        /// </summary>
        /// <param name="path">Path to .epub file</param>
        /// <returns></returns>
        public async static Task<Book> Create(string path)
        {
            Book book = new Book();
            book.Guid = Guid.NewGuid();
            path = MoveToInternalFolder(path, book.Guid.ToString());
            book.LinkToOrigin = path;

            EpubBook epubBook = await EpubReader.ReadBookAsync(path);
            book.Origin = epubBook;

            using (MemoryStream stream = new MemoryStream(epubBook.CoverImage))
            {
                Bitmap bitmap = new Bitmap(stream);
                string bitmapPath = $"{Directory.GetCurrentDirectory()}/covers/{book.Guid}.png";
                bitmap.Save(bitmapPath);
                book.LinkToCover = bitmapPath;
            }

            book.Author = epubBook.Author;
            book.Name = epubBook.Title;
            book.ChaptersCount = epubBook.ReadingOrder.Count;

            book.Save();

            return book;
        }

        /// <summary>
        /// Saves an instance of the class in json format in a special folder
        /// </summary>
        public void Save()
        {
            using (StreamWriter streamWriter = new StreamWriter(GetJsonFilePath()))
            {
                string json = JsonSerializer.Serialize(this);
                streamWriter.Write(json);
            }
        }

        /// <summary>
        /// Reads all jsonized instances of book classes from a special folder 
        /// </summary>
        /// <returns></returns>
        public static Book[] ReadAll() {
            string[] paths = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/books/");
            Book[] books = new Book[paths.Length];
            int i = 0;

            foreach (var path in paths)
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string data = streamReader.ReadToEnd();
                    books[i] = JsonSerializer.Deserialize<Book>(data);
                    i++;
                }
            }

            return books;
        }

        /// <summary>
        /// Reads async all jsonized instances of book classes from a special folder 
        /// </summary>
        /// <returns></returns>
        public async static Task<Book[]> ReadAllAsync()
        {
            string[] paths = Directory.GetFiles($"{Directory.GetCurrentDirectory()}/books/");
            Book[] books = new Book[paths.Length];
            int i = 0;

            foreach (var path in paths)
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string data = await streamReader.ReadToEndAsync();
                    books[i] = JsonSerializer.Deserialize<Book>(data);
                    i++;
                }
            }

            return books;
        }

        /// <summary>
        /// Removes a json record of a book as well as its ebup original from the application catalog.
        /// Does not remove the book cover.
        /// </summary>
        public void DeleteBook()
        {
            File.Delete(LinkToOrigin);
            File.Delete(GetJsonFilePath());      
        }

        private static string MoveToInternalFolder(string originPath, string newName)
        {
            string newPath = $"{Directory.GetCurrentDirectory()}/epubs/{newName}.epub";
            File.Move(originPath, newPath);
            return newPath;
        }

        private string GetJsonFilePath()
        {
            return $"{Directory.GetCurrentDirectory()}/books/{Path.GetFileName(LinkToOrigin)}.json";
        }
    }
}
