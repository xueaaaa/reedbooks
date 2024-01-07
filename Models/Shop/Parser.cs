using AngleSharp.Html.Dom;
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

            var html = await ConvertToHtml(Url);

            var matched = html.QuerySelectorAll("div").Where(elm => elm.ClassName != null && elm.ClassName.Contains("book-item")).ToList();
            if(matched == null ||  matched.Count == 0) return null;

            var books = new ObservableCollection<ParsedBook>();

            if (matched.Count > 10) matched = matched.Take(10).ToList();
            foreach (var match in matched)
            {
                var book = new ParsedBook();

                var titleElement = match.GetElementsByClassName("book-title").First();
                book.Name = titleElement.TextContent.Trim('\n', '\t', ' ');
                var link = titleElement.GetAttribute("href");
                book.Link = link;

                var bookIndividualPage = await ConvertToHtml(link);
                var downloadButtons = bookIndividualPage.QuerySelectorAll("a").Where(elm => elm.ClassName != null && elm.ClassName == "download-btn");

                foreach (var button in downloadButtons)
                {
                    if (!button.TextContent.ToLower().Contains("epub")) continue;
                    book.DownloadLink = button.GetAttribute("href");
                }

                book.Author = match.GetElementsByClassName("book-author").First().TextContent.Trim('\n', '\t', ' ');

                var rating = match.QuerySelector("rating");
                book.Rating = rating.GetAttribute("_avgrating") == string.Empty ? 0 : Convert.ToDouble(rating.GetAttribute("_avgrating").Replace('.', ','));

                book.RatedUsersNumber = rating.GetAttribute("_ratingsnumber") == string.Empty ? 0 : Convert.ToInt32(rating.GetAttribute("_ratingsnumber"));

                var genresRaw = match.GetElementsByClassName("book-genres");
                if (genresRaw != null && genresRaw.Count() > 0)
                {
                    var genres = match.GetElementsByClassName("book-genres").First().Children;
                    foreach (var genre in genresRaw)
                        book.Genres.Add(genre.TextContent.Trim('\n', '\t', ' '));
                }

                var bookYearElement = match.GetElementsByClassName("book-year");
                if (bookYearElement != null && bookYearElement.Count() > 0)
                {
                    var year = bookYearElement.First().QuerySelector("a")?.TextContent;
                    book.Year = Convert.ToInt32(year);
                }

                book.Description = bookIndividualPage.GetElementsByClassName("description").First().TextContent.Trim('\n', '\t', ' ');

                book.Cover = match.QuerySelector("img").GetAttribute("data-original");

                books.Add(book);
            }

            return books;
        }

        private async Task<IHtmlDocument> ConvertToHtml(string address)
        {
            var web = new HttpClient();
            var resp = await web.GetAsync(address);

            if (resp == null || resp.StatusCode != HttpStatusCode.OK) return null;

            var content = await resp.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            return parser.ParseDocument(content);
        }
    }
}
