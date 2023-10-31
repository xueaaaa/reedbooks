using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    public class EmotionalAssessment : ObservableObject
    {
        private Guid _guid;
        [Key]
        public Guid Guid
        {
            get => _guid;
            set
            {
                if (value != null)
                {
                    _guid = value;
                    OnPropertyChanged(nameof(Guid));
                }
            }
        }

        private Emote _start;
        public Emote Start
        {
            get => _start;
            set
            {
                _start = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(Start));
            }
        }

        private Emote _middle;
        public Emote Middle
        {
            get => _middle;
            set
            {
                _middle = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(Middle));
            }
        }

        private Emote _end;
        public Emote End
        {
            get => _end;
            set
            {
                _end = value;
                App.ApplicationContext.UpdateEnitity(this);
                OnPropertyChanged(nameof(End));
            }
        }

        public EmotionalAssessment()
        {
            Guid = Guid.NewGuid();
        }
    }
}
