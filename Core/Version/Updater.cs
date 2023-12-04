using System;
using System.IO;
using System.Linq;
using System.Net;

namespace ReedBooks.Core.Version
{
    public class Updater
    {
        private const string UPDATE_FILE_NAME = "ReedBooks.zip";
        public readonly string UPDATE_FILE_PATH = $"{Directory.GetCurrentDirectory()}\\{UPDATE_FILE_NAME}";

        private GitHubVersion _currentVersion;
        private Version _localVersion;

        public WebClient WebForDownloading { get; private set; }

        public Updater()
        {
            _currentVersion = new GitHubVersion();
            _localVersion = Version.Local;
            WebForDownloading = new WebClient();
        }

        /// <summary>
        /// Checks the local version of the program against the current version.
        /// </summary>
        /// <returns>true if there is a newer version, false otherwise</returns>
        public bool CheckForUpdates()
        {
            if (_localVersion < _currentVersion) return true;
            else return false;
        }

        public void InstallUpdate()
        {
            if(File.Exists(UPDATE_FILE_PATH)) File.Delete(UPDATE_FILE_PATH);
            WebForDownloading.DownloadFileAsync(new Uri(_currentVersion.Assets
                .Where(a => a.Name == UPDATE_FILE_NAME)
                .First().BrowserDownloadUrl), 
                UPDATE_FILE_PATH);
        }
    }
}
