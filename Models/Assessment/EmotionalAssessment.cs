using System.Text.Json.Serialization;

namespace ReedBooks.Models.Assessment
{
    public class EmotionalAssessment
    {
        [JsonPropertyName("start")] public Emote Start { get; set; }
        [JsonPropertyName("middle")]public Emote Middle { get; set; }
        [JsonPropertyName("end")] public Emote End { get; set; }
    }
}
