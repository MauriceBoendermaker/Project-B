using System.Text.Json;

namespace MegaBios
{
    public static class JsonFunctions
    {
        public static void WriteToJson<T>(string filePath, T data)
        {   
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }

        public static List<Movie> LoadMovies(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Movie>>(jsonString);
        }

        public static List<RoomShowing> LoadRoomShowings(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<RoomShowing>>(jsonString);
        }

        public static List<Account> LoadCustomers(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Account>>(jsonString);
        }

        public static List<Guest> LoadGuests(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                List<Guest> guests = JsonSerializer.Deserialize<List<Guest>>(jsonString, options);
                return guests;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Kan JSON niet deserialiseren naar type 'CinemaData': {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
                throw;
            }
        }

        public static List<List<Seat>> GenerateSeating(int height, int width)
        {
            List<List<Seat>> seating = new List<List<Seat>>(height);

            for (int i = 1; i <= height; i++)
            {
                seating.Add(new List<Seat>(height));

                for (int j = 1; j <= width; j++)
                {
                    Seat seat = new Seat();
                    seat.SeatNumber = $"{i}-{j}";
                    seat.SeatTaken = false;

                    if (i == 1 && (j == 1 || j == 2 || j == 3 || j == width || j == width - 1 || j == width - 2))
                    {
                        seat.SeatType = "handicap";
                        seat.Price = 10.00;
                    }
                    else if (i != 1 && i % 2 != 0 && (j == 1 || j == 2 || j == width || j == width - 1))
                    {
                        seat.SeatType = "love seat";
                        seat.Price = 20.00;
                    }
                    else
                    {
                        seat.SeatType = "normal";
                        seat.Price = 10.00;
                    }

                    seating[i-1].Add(seat);
                }
            }

            return seating;
        }
    }
}
