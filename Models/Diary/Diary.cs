using ReedBooks.Core;
using ReedBooks.Models.Assessment;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReedBooks.Models.Diary
{
    public class ReadingDiary : ObservableObject
    {
        [JsonPropertyName("begin_reading_at")] public DateTime BeginReadingAt { get; set; }
        [JsonPropertyName("end_reading_at")] public DateTime EndReadingAt { get; set; }
        [JsonPropertyName("emotional_assessment")] public EmotionalAssessment EmotionalAssessment { get; set; }
        [JsonPropertyName("book_assessment")] public BookAssessment BookAssessment { get; set; }
        [JsonPropertyName("plot_brief_retelling")] public string PlotBriefRetelling { get; set; }
        [JsonPropertyName("quotes")] public List<Quote> Quotes { get; set; }
    }
}
