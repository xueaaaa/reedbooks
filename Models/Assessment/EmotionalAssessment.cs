using ReedBooks.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    /// <summary>
    /// A class for evaluating a book on emotional feelings
    /// </summary>
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
        /// <summary>
        /// Evaluation at the beginning of the book
        /// </summary>
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
        /// <summary>
        /// A mid-book evaluation
        /// </summary>
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
        /// <summary>
        /// Evaluation at the end of the book
        /// </summary>
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
