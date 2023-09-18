using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReedBooks.Models.Assessment
{
    public class BookAssessment
    {
        private const ushort ASSESMENTS_TOTAL_NUMBER = 6;

        [Range(1, 5)] [JsonPropertyName("plot_originality")] public ushort PlotOriginality { get; set; }
        [Range(1, 5)] [JsonPropertyName("characters")] public ushort Characters { get; set; }
        [Range(1, 5)] [JsonPropertyName("world_inside_book")] public ushort WorldInsideBook { get; set; }
        [Range(1, 5)] [JsonPropertyName("love_line")] public ushort LoveLine { get; set; }
        [Range(1, 5)] [JsonPropertyName("humor")] public ushort Humor { get; set; }
        [Range(1, 5)] [JsonPropertyName("meaning_fulness")] public ushort Meaningfulness { get; set; }

        /// <summary>
        /// Calculates the book's overall grade based on 6 individual grades, rounded to tenths
        /// </summary>
        /// <returns></returns>
        public float CalculateArithmeticAverage()
        {
            float rawAverage = (PlotOriginality 
                + Characters 
                + WorldInsideBook 
                + LoveLine 
                + Humor 
                + Meaningfulness) 
                / ASSESMENTS_TOTAL_NUMBER;

            return (float)Math.Round(rawAverage, 1);
        }
    }
}
