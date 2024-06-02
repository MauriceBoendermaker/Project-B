using System.Text.Json.Serialization;

namespace MegaBios
{
    public class Movie
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("genre")]
        public string Genre { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }
    }
}