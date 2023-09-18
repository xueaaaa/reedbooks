using ReedBooks.Core;
using System;
using System.Collections.Generic;

namespace ReedBooks.Models.Diary
{
    public class Diary : ObservableObject
    {
        public DateTime BeginReadingAt { get; set; }
        public DateTime EndReadingAt { get; set; }
        public BookAssessment Assesment { get; set; }
        public string PlotBriefRetelling { get; set; }
        public List<Quote> Quotes { get; set; }
    }
}
