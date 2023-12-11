using ReedBooks.Core;
using ReedBooks.Core.Version;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class UpdateWindowViewModel : ObservableObject
    {
        private GitHubVersion _version;
        public GitHubVersion Version 
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }

        private Updater _updater;
        public Updater Updater
        {
            get => _updater;
            set
            {
                _updater = value;
                OnPropertyChanged(nameof(Updater));
            }
        }

        private double _progressBarPercentage;
        public double ProgressBarPercentage
        {
            get => _progressBarPercentage;
            set
            {
                _progressBarPercentage = value;
                OnPropertyChanged(nameof(ProgressBarPercentage));
            }
        }

        public ICommand UpdateCommand { get; }

        public string Header
        {
            get => $"{Application.Current.Resources["u_header"]} {Version.Name}";
        }

        public UpdateWindowViewModel()
        {
            UpdateCommand = new RelayCommand(obj => Update());

            Version = new GitHubVersion();
            Updater = App.Updater;
            Updater.WebForDownloading.DownloadProgressChanged += WebForDownloading_DownloadProgressChanged;
            Updater.WebForDownloading.DownloadFileCompleted += WebForDownloading_DownloadFileCompleted;

            if (Properties.Settings.Default.UpdateAutomatically) Update();
        }

        private void WebForDownloading_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start($"{Directory.GetCurrentDirectory()}\\updater.exe", Updater.UPDATE_FILE_PATH);
            Process.GetCurrentProcess().Kill(); 
        }

        private void Update()
        {
            Updater.InstallUpdate();
        }

        private void WebForDownloading_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBarPercentage = e.ProgressPercentage;
        }
    }
}
