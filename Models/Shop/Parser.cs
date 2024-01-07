using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReedBooks.Models.Shop
{
    public class Parser
    {
        public string Url { get; set; }

        public async Task<List<ParsedBook>> Parse(string searchQuery)
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

            var books = new List<ParsedBook>();
            foreach (var match in matched)
            {
                var book = new ParsedBook();

                book.Name = match.GetElementsByClassName("book-title").First().TextContent;
                book.Author = match.GetElementsByClassName("book-author").First().TextContent;
                var rating = match.QuerySelector("rating").GetAttribute("_avgrating");
                book.Rating = Convert.ToDouble(rating.Replace('.', ','));

                var genresRaw = match.GetElementsByClassName("book-genres").First().Children;
                foreach (var genre in genresRaw)
                    book.Genres.Add(genre.TextContent);

                book.Year = Convert.ToInt32(match.GetElementsByClassName("book-year").First().QuerySelector("a").TextContent);
                book.Description = match.GetElementsByClassName("description").First().TextContent;
                book.Cover = match.QuerySelector("img").GetAttribute("src");

                books.Add(book);
            }

            return null;
        }
    }
}
