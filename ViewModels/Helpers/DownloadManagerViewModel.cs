using System.IO;
using System.Net;
using System;
using System.Windows;
using System.Windows.Input;
using ReedBooks.Core;
using ReedBooks.Models.Shop;
using ReedBooks.Models.Book;

namespace ReedBooks.ViewModels.Helpers
{
    public class DownloadManagerViewModel : ObservableObject
    {
        #region Properties
        public delegate void DownloadCompletedHandler(Book book);
        public event DownloadCompletedHandler DownloadCompleted;

        private Visibility _downloadManagerVisibility;
        public Visibility DownloadManagerVisibility
        {
            get => _downloadManagerVisibility;
            set
            {
                _downloadManagerVisibility = value;
                OnPropertyChanged(nameof(DownloadManagerVisibility));
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

        private double _megabytesReceived;
        public double MegabytesReceived
        {
            get => _megabytesReceived;
            set
            {
                _megabytesReceived = value;
                OnPropertyChanged(nameof(MegabytesReceived));
            }
        }

        private double _totalMegabytes;
        public double TotalMegabytes
        {
            get => _totalMegabytes;
            set
            {
                _totalMegabytes = value;
                OnPropertyChanged(nameof(TotalMegabytes));
            }
        }

        private ParsedBook _book;
        public ParsedBook Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
            }
        }
        #endregion

        #region Commands
        public ICommand DownloadCommand { get; }
        public ICommand CloseCommand { get; }
        #endregion

        public DownloadManagerViewModel()
        {
            DownloadManagerVisibility = Visibility.Collapsed;

            DownloadCommand = new RelayCommand(obj => Download(obj));
            CloseCommand = new RelayCommand(obj => Close());
        }

        public void Download(object param)
        {
            ParsedBook book = (ParsedBook)param;
            Book = book;
            var link = $"{Directory.GetCurrentDirectory()}{StorageManager.EPUBS_DIRECTORY}{book.Name.Replace(':', '-')}.epub";

            using (var client = new WebClient())
            {
                DownloadManagerVisibility = Visibility.Visible;
                client.DownloadFileAsync(new Uri(book.DownloadLink), link);

                client.DownloadProgressChanged += (o, e) =>
                {
                    ProgressBarPercentage = e.ProgressPercentage;
                    MegabytesReceived = e.BytesReceived / Math.Pow(2, 20);
                    TotalMegabytes = e.TotalBytesToReceive / Math.Pow(2, 20);
                };

                client.DownloadFileCompleted += (o, e) =>
                {
                    var downloaded = new Book(link);
                    DownloadCompleted?.Invoke(downloaded);
                };
            }
        }

        public void Close()
        {
            Book = null;
            DownloadManagerVisibility = Visibility.Collapsed;
        }
    }
}
