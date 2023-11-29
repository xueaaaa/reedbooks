using System;
using System.IO;
using System.Linq;

namespace ReedBooks.Core
{
    public class StorageManager
    {
        public const string COVERS_DIRECTORY = "\\covers\\";
        public const string EPUBS_DIRECTORY = "\\epubs\\";

        /// <summary>
        /// Returns the size of unused (of which there are no records in the database) files, in MB
        /// </summary>
        public double UnusedFilesSize
        {
            get
            {
                double size = 0;

                foreach (var cover in Directory.GetFiles($"{Directory.GetCurrentDirectory()}{COVERS_DIRECTORY}"))
                {
                    int count = App.ApplicationContext.Books.Where(b => b.LinkToCover == cover).Count();
                    if (count == 0) size += (new FileInfo(cover).Length) / Math.Pow(1024, 2);
                }

                foreach (var epub in Directory.GetFiles($"{Directory.GetCurrentDirectory()}{EPUBS_DIRECTORY}"))
                {
                    int count = App.ApplicationContext.Books.Where(b => b.LinkToOrigin == epub).Count();
                    if (count == 0) size += (new FileInfo(epub).Length) / Math.Pow(1024, 2);
                }

                return size;
            }
        }

        /// <summary>
        /// Deletes all files no longer used by the program
        /// </summary>
        public void DeleteUnusedFiles()
        {
            foreach (var cover in Directory.GetFiles($"{Directory.GetCurrentDirectory()}{COVERS_DIRECTORY}"))
            {
                int count = App.ApplicationContext.Books.Where(b => b.LinkToCover == cover).Count();
                if (count == 0) File.Delete(cover);
            }

            foreach (var cover in Directory.GetFiles($"{Directory.GetCurrentDirectory()}{EPUBS_DIRECTORY}"))
            {
                int count = App.ApplicationContext.Books.Where(b => b.LinkToOrigin == cover).Count();
                if (count == 0) File.Delete(cover);
            }
        }

        /// <summary>
        /// Deletes all books loaded into the database
        /// </summary>
        public async void DeleteAllBooks()
        {
            App.ApplicationContext.RemoveRange(App.ApplicationContext.Books);
            await App.ApplicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks the current application directory for folders required for its operation and creates them if they are missing
        /// </summary>
        public static void EnsureCreated()
        {
            var coversPath = $"{Directory.GetCurrentDirectory()}{COVERS_DIRECTORY}";
            var epubsPath = $"{Directory.GetCurrentDirectory()}{EPUBS_DIRECTORY}";

            if (!Directory.Exists(coversPath))
                Directory.CreateDirectory(coversPath);
            if (!Directory.Exists(epubsPath))
                Directory.CreateDirectory(epubsPath);
        }
    }
}
