using ReedBooks.Core;

namespace ReedBooks.Models.Diary
{
    public class Quote : ObservableObject
    {
        public string Data { get; set; }
        public string Author { get; set; }
        public string LocationInBook { get; set; }
    }
}
