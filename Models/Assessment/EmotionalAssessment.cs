using System;
using System.ComponentModel.DataAnnotations;

namespace ReedBooks.Models.Assessment
{
    public class EmotionalAssessment
    {
        [Key] public Guid Guid { get; set; }
        public Emote Start { get; set; }
        public Emote Middle { get; set; }
        public Emote End { get; set; }

        public EmotionalAssessment()
        {
            Guid = Guid.NewGuid();
        }
    }
}
