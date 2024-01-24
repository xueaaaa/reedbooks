using ReedBooks.Properties;
using System;
using System.Windows.Threading;

namespace ReedBooks.Core
{
    public class TimeGoalController : ObservableObject
    {
        private DispatcherTimer _timer;

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
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += (_, e) =>
            {
                CountedMinutes++;
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
            _timer.Start();
            OnPropertyChanged(nameof(CountedMinutes));
        }

        public void StopCounter()
        {
            _timer.Stop();
            Save();
            OnPropertyChanged(nameof(CountedMinutes));
        }
        
        public void Save()
        {
            Settings.Default.CurrentReadingTime = (ushort)CountedMinutes;
            Settings.Default.Save();
        }
    }
}
