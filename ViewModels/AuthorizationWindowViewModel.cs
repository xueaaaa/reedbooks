using ReedBooks.Core;
using ReedBooks.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace ReedBooks.ViewModels
{
    public class AuthorizationWindowViewModel : ObservableObject
    {
        public delegate void StateHandler();
        public event StateHandler Success;
        public event StateHandler Reset;

        private string[] _enteredPassword;
        public string[] EnteredPassword
        {
            get => _enteredPassword;
            set
            {
                _enteredPassword = value;
                OnPropertyChanged(nameof(EnteredPassword));
            }
        }

        private int _currentPosition;
        public int CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        private bool? _failed;
        public bool? Failed
        {
            get => _failed;
            set
            {
                _failed = value;
                OnPropertyChanged(nameof(Failed));
            }
        }

        public ICommand EnterSymbolCommand { get; }
        public ICommand EraseSymbolCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public AuthorizationWindowViewModel()
        {
            EnteredPassword = new string[4];
            CurrentPosition = 0;

            EnterSymbolCommand = new RelayCommand(obj => EnterSymbol(obj));
            EraseSymbolCommand = new RelayCommand(obj => EraseSymbol());
            ResetPasswordCommand = new RelayCommand(obj => ResetPassword());    
        }

        public void EnterSymbol(object param)
        {
            Failed = null;

            if(CurrentPosition >= EnteredPassword.Length)
            {
                CurrentPosition = 0;
                EnteredPassword = new string[4];
            }

            EnteredPassword[CurrentPosition] = Convert.ToString(param);
            CurrentPosition++;
            OnPropertyChanged(nameof(EnteredPassword));

            if(CurrentPosition == EnteredPassword.Length)
            {
                string correctPassword = Properties.Settings.Default.Password;
                if (correctPassword == string.Join(string.Empty, EnteredPassword)) 
                {
                    Failed = false;
                    Success?.Invoke();
                }
                else Failed = true;
            }
        }

        public void EraseSymbol()
        {
            if (CurrentPosition > 0)
            {
                EnteredPassword[CurrentPosition - 1] = string.Empty;
                CurrentPosition--;
                OnPropertyChanged(nameof(EnteredPassword));
            }
        }

        public void ResetPassword()
        {
            if(new DialogWindow(Application.Current.Resources["dialog_forgot_password_title"].ToString(),
                Application.Current.Resources["dialog_forgot_password_content"].ToString()).ShowDialog() == true)
            {
                App.StorageManager.DeleteAllBooks();
                Properties.Settings.Default.Reset();
                Reset?.Invoke();
            }
        }
    }
}
