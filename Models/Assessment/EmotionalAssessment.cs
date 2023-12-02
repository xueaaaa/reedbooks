using ReedBooks.Models.Database;

namespace ReedBooks.Models.Assessment
{
    /// <summary>
    /// A class for evaluating a book on emotional feelings
    /// </summary>
    public class EmotionalAssessment : DatabaseObject
    {
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
                Update();
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
                Update();
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
                Update();
                OnPropertyChanged(nameof(End));
            }
        }
    }
}
