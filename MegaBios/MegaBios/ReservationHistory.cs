using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class ReservationHistory
    {
        [JsonPropertyName("reservation_number")]
        public string ReservationNumber { get; set; }

        [JsonPropertyName("movie_title")]
        public string MovieTitle { get; set; }

        [JsonPropertyName("reservation_date")]
        public DateTime ReservationDate { get; set; }
    }
}