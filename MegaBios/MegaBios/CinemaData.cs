using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class CinemaData
    {
        [JsonPropertyName("cinema_rooms")]
        public List<CinemaRoom> CinemaRooms { get; set; }
    }
}