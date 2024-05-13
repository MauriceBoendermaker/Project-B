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
        
        // TODO: Add property that saves the reserved seat(s) so it can be iterated over to unoccupy seats in case of ticket cancellation
    }
}