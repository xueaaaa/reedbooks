using ReedBooks.Core;
using ReedBooks.Models.Diary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using VersOne.Epub;

namespace ReedBooks.Models.Book
{
    public class Book : ObservableObject
    {
        private Guid _guid;
        [Key] public Guid Guid 
        { 
            get => _guid; 
            set 
            {
                if (value != null)
                {
                    _guid = value;
                    OnPropertyChanged(nameof(Guid));
                }
            } 
        }

        private ReadingDiary _boundDiary;
        public ReadingDiary BoundDiary 
        { 
            get => _boundDiary; 
            set
            {
                if (value != null)
                {
                    _boundDiary = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(BoundDiary));
                }
            }
        }

        private string _name;
        public string Name 
        { 
            get => _name; 
            set
            {
                if(value != null)
                {
                    _name = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _author;
        public string Author 
        {
            get => _author;
            set
            {
                if(value != null)
                {
                    _author = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        private int _chaptersCount;
        public int ChaptersCount 
        { 
            get => _chaptersCount; 
            set
            {
                if (value >= 0)
                {
                    _chaptersCount = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(ChaptersCount));
                }
            }
        }

        private string _genre;
        public string Genre 
        {
            get => _genre;
            set
            {
                if(value != null)
                {
                    _genre = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(Genre));
                }
            }
        }

        private string _linkToOrigin;
        public string LinkToOrigin 
        {
            get => _linkToOrigin;
            set
            {
                if (value != null)
                {
                    _linkToOrigin = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(LinkToOrigin));
                }
            }
        }

        private string _linkToCover;
        public string LinkToCover 
        {
            get => _linkToCover; 
            set
            {
                if(value != null)
                {
                    _linkToCover = value;
                    App.ApplicationContext.UpdateEnitity(this);
                    OnPropertyChanged(nameof(LinkToCover));
                }
            }
        }

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

            await App.ApplicationContext.AddEntityAsync(book);

            return book;
        }

        /// <summary>
        /// Saves a record of the current object to the local database
        /// </summary>
        public async Task<int> Save()
        {
            App.ApplicationContext.Books.Update(this);
            return await App.ApplicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Reads all book records from the database and returns them as an ObservableCollection
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Book> ReadAll()
        {
            return App.ApplicationContext.Books.Local.ToObservableCollection();
        }

        /// <summary>
        /// Removes a record of a book from the database as well as its ebup original from the application catalog.
        /// Does not remove the book cover.
        /// </summary>
        public async void Delete()
        {
            await App.ApplicationContext.RemoveEntityAsync(this);
            File.Delete(LinkToOrigin);
        }

        private static string MoveToInternalFolder(string originPath, string newName)
        {
            string newPath = $"{Directory.GetCurrentDirectory()}/epubs/{newName}.epub";
            File.Move(originPath, newPath);
            return newPath;
        }
    }
}
