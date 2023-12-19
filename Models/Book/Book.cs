using ReedBooks.Core;
using ReedBooks.Models.Database;
using ReedBooks.Models.Diary;
using ReedBooks.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VersOne.Epub;

namespace ReedBooks.Models.Book
{
    public class Book : DatabaseObject
    {
        private ReadingDiary _boundDiary;
        public ReadingDiary BoundDiary 
        { 
            get => _boundDiary; 
            set
            {
                if (value != null)
                {
                    _boundDiary = value;
                    Update();
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
                    Update();
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
                    Update();
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        private int _chaptersCount;
        public int ChaptersCount 
        { 
            get => _chaptersCount; 
            private set
            {
                if (value >= 0)
                {
                    _chaptersCount = value;
                    Update();
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
                    Update();
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
                    Update();
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
                    Update();
                    OnPropertyChanged(nameof(LinkToCover));
                }
            }
        }

        private Position _lastReadingPosition;
        public Position LastReadingPosition
        {
            get => _lastReadingPosition;
            set
            {
                _lastReadingPosition = value;
                OnPropertyChanged(nameof(LastReadingPosition));
            }
        }

        public Book() 
        { 
            
        }

        public Book(string path)
        {
            Create();
            _boundDiary = new ReadingDiary(Guid);

            path = MoveToInternalFolder(path, Guid.ToString());
            _linkToOrigin = path;

            EpubBook epubBook = EpubReader.ReadBook(path);

            using (MemoryStream stream = new MemoryStream(epubBook.CoverImage))
            {
                Bitmap bitmap = new Bitmap(stream);
                string bitmapPath = $"{Directory.GetCurrentDirectory()}{StorageManager.COVERS_DIRECTORY}{Guid}.png";
                bitmap.Save(bitmapPath);
                _linkToCover = bitmapPath;
            }

            _author = epubBook.Author;
            _name = epubBook.Title;
            _chaptersCount = epubBook.Content.Html.Local.Count;

            Update();
        }

        public override Task<int> RemoveAsync()
        {
            var collections = App.ApplicationContext.Collections.Where(c => c.LinkedBooks.Contains(this.Guid.ToString())).ToList();
            foreach (var item in collections)
                item.RemoveBook(this.Guid);

            return base.RemoveAsync();
        }

        /// <summary>
        /// Reads all book records from the database and returns them as an ObservableCollection
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Book>> ReadAll()
        {
            var books = App.ApplicationContext.Books.Local.ToObservableCollection();

            foreach (var book in books)
            {
                if (book != null)
                {
                    await App.ApplicationContext.Entry(book)
                          .Reference(b => b.BoundDiary)
                          .LoadAsync();

                    await App.ApplicationContext.Entry(book)
                        .Reference(b => b.LastReadingPosition)
                        .LoadAsync();

                    await App.ApplicationContext.Entry(book.BoundDiary)
                          .Reference(d => d.EmotionalAssessment)
                          .LoadAsync();

                    await App.ApplicationContext.Entry(book.BoundDiary)
                          .Reference(d => d.BookAssessment)
                          .LoadAsync();
                }
            }

            return books;
        }

        /// <summary>
        /// Loads a chapter from epub
        /// </summary>
        /// <param name="contentFilePath">Title of the chapter's html file</param>
        /// <returns>HtmlPanel containing the formatted text of the chapter</returns>
        public string LoadChapter(string contentFilePath)
        {
            var book = GetEpub();
            var content = book.ReadingOrder.Where(c => c.FilePath == contentFilePath).First();
            return content.Content;
        }

        /// <summary>
        /// Creates a list that corresponds to the epub book's navigation
        /// </summary>
        /// <returns>List with items of type NavigationItem for use in TreeView</returns>
        public List<NavigationItem> LoadNavigation()
        {
            var book = GetEpub();
            var navigaion = new List<NavigationItem>();

            foreach (var item in book.Navigation)
            {
                var navItem = LoadNavigationItem(item);
                navigaion.Add(navItem);
            }

            return navigaion;
        }

        public void SetStartReadingDate()
        {
            if (BoundDiary.BeginReadingAt == DateTime.MinValue) BoundDiary.BeginReadingAt = DateTime.Now;
        }

        public void SetLastReadingDate()
        {
            BoundDiary.LastReadingAt = DateTime.Now;
        }

        public void MarkAsRead()
        {
            BoundDiary.EndReadingAt = DateTime.Now;
            BoundDiary.ReadingIsOver = true;
        }

        /// <summary>
        /// Gets the content of the book
        /// </summary>
        /// <returns>Content in the form of an EpubBook class</returns>
        public EpubBook GetEpub()
        {
            return EpubReader.ReadBook(LinkToOrigin);
        }

        private static string MoveToInternalFolder(string originPath, string newName)
        {
            string newPath = $"{Directory.GetCurrentDirectory()}{StorageManager.EPUBS_DIRECTORY}{newName}.epub";
            File.Move(originPath, newPath);
            return newPath;
        }

        private NavigationItem LoadNavigationItem(EpubNavigationItem item)
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
