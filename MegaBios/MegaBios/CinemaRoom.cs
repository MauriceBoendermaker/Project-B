using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class CinemaRoom
    {
        [JsonPropertyName("room_number")]
        public string RoomNumber { get; set; }

        [JsonPropertyName("seating")]
        public List<List<Seat>> Seating { get; set; }

        [JsonPropertyName("schedule")]
        public Dictionary<string, List<MovieSchedule>> Schedule { get; set; }
    }

    public class Seat
    {
        [JsonPropertyName("seat_number")]
        public string SeatNumber { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class MovieSchedule
    {
        [JsonPropertyName("movie")]
        public string Movie { get; set; }

        [JsonPropertyName("start_time")]
        public string StartTime { get; set; }
    }
}
