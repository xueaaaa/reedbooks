using ReedBooks.Core;

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

        private bool _dialogResult;
        public bool DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }

        public DialogWindowViewModel() 
        {

        }

        public DialogWindowViewModel(string title, string content) 
        {
            Title = title;
            Content = content;
        }
    }
}
