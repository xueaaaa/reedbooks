using ReedBooks.Core;
using System.Text.Json.Serialization;

namespace ReedBooks.Models.Diary
{
    public class Quote : ObservableObject
    {
        [JsonPropertyName("data")] public string Data { get; set; }
        [JsonPropertyName("author")] public string Author { get; set; }
        [JsonPropertyName("location_in_book")] public string LocationInBook { get; set; }

        public Quote(string data)
        {
            Data = data;
        }

        public Quote(string data, string author) : this(data)
        {
            Author = author;
        }

        public Quote(string data, string author, string locationInBook) : this(data, author)
        {
            LocationInBook = locationInBook;
        }
    }
}
