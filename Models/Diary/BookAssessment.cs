using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Diary
{
    public class BookAssessment
    {
        [Range(1, 5)] public int Beginning { get; set; }
        [Range(1, 5)] public int Middle { get; set; }
        [Range(1, 5)] public int End { get; set; }
        [Range(1, 5)] public int PlotOriginality { get; set; }
        [Range(1, 5)] public int Characters { get; set; }
        [Range(1, 5)] public int WorldInsideBook { get; set; }
        [Range(1, 5)] public int LoveLine { get; set; }
        [Range(1, 5)] public int Humor { get; set; }
        [Range(1, 5)] public int Meaningfulness { get; set; }
    }
}
