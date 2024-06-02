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

    public class RoomShowing
    {
        [JsonPropertyName("room_number")]
        public string RoomNumber { get; set; }

        [JsonPropertyName("seating")]
        public List<List<Seat>> Seating { get; set; }

        [JsonPropertyName("movie_playing")]
        public string Movie { get; set; }

        [JsonPropertyName("show_time")]
        public DateTime ShowingTime { get; set; }

        [JsonPropertyName("in_maintenance")]
        public bool InMaintenance { get; set; }

        public RoomShowing()
        {
            // Default constructor nodig voor deserialization
        }

        public RoomShowing(string roomNumber, List<List<Seat>> seating, string movie, DateTime showTime, bool inMaintenance)
        {
            RoomNumber = roomNumber;
            Seating = seating;
            Movie = movie;
            ShowingTime = showTime;
            InMaintenance = inMaintenance;
        }
    }

    public class Seat
    {
        [JsonPropertyName("seat_number")]
        public string SeatNumber { get; set; } 

        [JsonPropertyName("is_taken")]
        public bool SeatTaken { get; set; }

        [JsonPropertyName("type")]
        public string SeatType { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

    }

    public class MovieSchedule
    {
        [JsonPropertyName("movie")]
        public string Movie { get; set; }

        [JsonPropertyName("start_time")]
        public string StartTime { get; set; }

        [JsonPropertyName("price_multiplier")]
        public double PriceMultiplier { get; set; } = 1.0; // Standaard multiplier
    }
}
