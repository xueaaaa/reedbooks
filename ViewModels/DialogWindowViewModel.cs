using ReedBooks.Core;
using System.Windows;

namespace ReedBooks.ViewModels
{
    public class DialogWindowViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value; 
                OnPropertyChanged(nameof(Content));
            }
        }

        private Visibility _showCancel;
        public Visibility ShowCancel
        {
            get => _showCancel;
            set
            {
                _showCancel = value;
                OnPropertyChanged(nameof(ShowCancel));
            }
        }

        public DialogWindowViewModel() 
        {

        }

        public DialogWindowViewModel(string title, string content, Visibility showCancel) 
        {
            Title = title;
            Content = content;
            ShowCancel = showCancel;
        }
    }
}
