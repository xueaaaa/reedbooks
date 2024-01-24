using ReedBooks.Properties;
using System;
using System.Windows.Threading;

namespace ReedBooks.Core
{
    public class TimeGoalController : ObservableObject
    {
        private DispatcherTimer _globalTimer;

        private int _countedMinutes;
        public int CountedMinutes
        {
            get 
            {
                if (_countedMinutes == 0) return Settings.Default.CurrentReadingTime;
                return _countedMinutes; 
            }
            set
            {
                _countedMinutes = value;
                OnPropertyChanged(nameof(CountedMinutes));  
            }
        }

        private int _currentBookCountedMinutes;
        public int CurrentBookCountedMinutes
        {
            get => _currentBookCountedMinutes;
            set
            {
                _currentBookCountedMinutes = value;
                OnPropertyChanged(nameof(CurrentBookCountedMinutes));
            }
        }

        private double _timeGoalPercent;
        public double TimeGoalPercent
        {
            get => _timeGoalPercent;
            set
            {
                _timeGoalPercent = value;
                OnPropertyChanged(nameof(TimeGoalPercent));
            }
        }

        public TimeGoalController()
        {
            _globalTimer = new DispatcherTimer();
            _globalTimer.Interval = TimeSpan.FromSeconds(60);
            _globalTimer.Tick += (_, e) =>
            {
                CountedMinutes++;
                CurrentBookCountedMinutes++;
                TimeGoalPercent = Math.Round(CountedMinutes / (Settings.Default.TimeGoal / 100D));
            };


            if (Settings.Default.LastOpenDay.Date != DateTime.Today)
            {
                Settings.Default.LastOpenDay = DateTime.Now;
                Settings.Default.CurrentReadingTime = 0;
                Settings.Default.Save();
            }
            else CountedMinutes = Settings.Default.CurrentReadingTime;

            TimeGoalPercent = Math.Round(CountedMinutes / (Settings.Default.TimeGoal / 100D));
        }

        public void StartCounter()
        {
            _globalTimer.Start();
        }

        public void StopCounter()
        {
            _globalTimer.Stop();
            CurrentBookCountedMinutes = 0;
            Save();
        }
        
        public void Save()
        {
            Settings.Default.CurrentReadingTime = (ushort)CountedMinutes;
            Settings.Default.Save();
        }
    }
}
