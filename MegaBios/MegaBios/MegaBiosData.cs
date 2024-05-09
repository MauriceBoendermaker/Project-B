using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MegaBios
{
    public class MegaBiosData
    {
        [JsonPropertyName("amount_of_rooms")]
        public int AmountOfRooms { get; set; }
    }
}