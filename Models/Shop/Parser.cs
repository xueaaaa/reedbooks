using AngleSharp.Html.Parser;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReedBooks.Models.Shop
{
    public class Parser
    {
        public string Url { get; set; }

        public async Task<ObservableCollection<ParsedBook>> Parse(string searchQuery)
        {
            Url = "https://topliba.com/?q=" + searchQuery.Replace(' ', '+');

            var web = new HttpClient();
            var resp = await web.GetAsync(Url);

            if (resp == null || resp.StatusCode != HttpStatusCode.OK) return null;

            var content = await resp.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            var html = parser.ParseDocument(content);

            var matched = html.QuerySelectorAll("div").Where(elm => elm.ClassName != null && elm.ClassName.Contains("book-item")).ToList();
            if(matched == null ||  matched.Count == 0) return null;

            var books = new ObservableCollection<ParsedBook>();

            if (matched.Count > 20) matched = matched.Take(20).ToList();
            foreach (var match in matched)
            {
                var book = new ParsedBook();

                book.Name = match.GetElementsByClassName("book-title").First().TextContent.Trim('\n', '\t', ' ');
                book.Author = match.GetElementsByClassName("book-author").First().TextContent.Trim('\n', '\t', ' ');
                var rating = match.QuerySelector("rating");
                book.Rating = rating.GetAttribute("_avgrating") == string.Empty ? 0 : Convert.ToDouble(rating.GetAttribute("_avgrating").Replace('.', ','));
                book.RatedUsersNumber = rating.GetAttribute("_ratingsnumber") == string.Empty ? 0 : Convert.ToInt32(rating.GetAttribute("_ratingsnumber"));

                var genresRaw = match.GetElementsByClassName("book-genres").First().Children;
                foreach (var genre in genresRaw)
                    book.Genres.Add(genre.TextContent.Trim('\n', '\t', ' '));

                var bookYearElement = match.GetElementsByClassName("book-year");
                if (bookYearElement != null && bookYearElement.Count() > 0)
                {
                    var year = bookYearElement.First().QuerySelector("a")?.TextContent;
                    book.Year = Convert.ToInt32(year);
                }
                book.Description = match.GetElementsByClassName("description").First().TextContent.Trim('\n', '\t', ' ');
                book.Cover = match.QuerySelector("img").GetAttribute("data-original");

                books.Add(book);
            }

            return books;
        }
    }
}
