using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    public class BookAssessment
    {
        private const ushort ASSESMENTS_TOTAL_NUMBER = 6;

        [Key] public Guid Guid { get; set; }
        [Range(1, 5)] public ushort PlotOriginality { get; set; }
        [Range(1, 5)] public ushort Characters { get; set; }
        [Range(1, 5)] public ushort WorldInsideBook { get; set; }
        [Range(1, 5)] public ushort LoveLine { get; set; }
        [Range(1, 5)] public ushort Humor { get; set; }
        [Range(1, 5)] public ushort Meaningfulness { get; set; }

        public BookAssessment() 
        {
            Guid = Guid.NewGuid();
        }

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
