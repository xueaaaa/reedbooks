﻿using System;
using System.IO;
using System.Linq;

namespace ReedBooks.Core
{
    public class StorageManager
    {
        public const string COVERS_DIRECTORY = "\\covers\\";
        public const string EPUBS_DIRECTORY = "\\epubs\\";
        public const string SHARED_DIRECTORY = "\\share\\";

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
        /// Returns the amount of memory occupied by the program, in MB
        /// </summary>
        public double SummarySize
        {
            get
            {
                return GetFromDirectory(Directory.GetCurrentDirectory());
            }
        }

        public StorageManager()
        {
            StorageManager.EnsureCreated();
            if (Properties.Settings.Default.DeleteUnusedAutomatically) DeleteUnusedFiles();
        }

        public double GetFromDirectory(string path)
        {
            double size = 0;

            foreach (var directory in Directory.GetDirectories(path))
                size += GetFromDirectory(directory);

            foreach (var file in Directory.GetFiles(path))
                size += (new FileInfo(file).Length) / Math.Pow(1024, 2);

            return size;
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
            App.ApplicationContext.RemoveRange(App.ApplicationContext.Collections);
            await App.ApplicationContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks the current application directory for folders required for its operation and creates them if they are missing
        /// </summary>
        public static void EnsureCreated()
        {
            var coversPath = $"{Directory.GetCurrentDirectory()}{COVERS_DIRECTORY}";
            var epubsPath = $"{Directory.GetCurrentDirectory()}{EPUBS_DIRECTORY}";
            var resourcesPath = $"{Directory.GetCurrentDirectory()}\\resources";
            var themesPath = $"{resourcesPath}\\themes";
            var sharedPath = $"{Directory.GetCurrentDirectory()}{SHARED_DIRECTORY}";

            if (!Directory.Exists(coversPath))
                Directory.CreateDirectory(coversPath);
            if (!Directory.Exists(epubsPath))
                Directory.CreateDirectory(epubsPath);
            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);
            if (!Directory.Exists(themesPath))
                Directory.CreateDirectory(themesPath);
            if(!Directory.Exists(SHARED_DIRECTORY))
                Directory.CreateDirectory(sharedPath);

            if (File.Exists(App.Updater.UPDATE_FILE_PATH)) File.Delete(App.Updater.UPDATE_FILE_PATH);
        }
    }
}
