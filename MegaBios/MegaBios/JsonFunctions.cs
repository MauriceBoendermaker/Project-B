using System.Text.Json;

namespace MegaBios
{
    public static class JsonFunctions
    {
        public static void WriteToJson<T>(string filePath, T data)
        {   
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                filePath = "../../../../MegaBios/obj/Debug/net8.0/" + filePath;
            }
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }

        public static List<Movie> LoadMovies(string filePath)
        {
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                filePath = "../../../../MegaBios/obj/Debug/net8.0/" + filePath;
            }            
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Movie>>(jsonString);
        }

        public static List<RoomShowing> LoadRoomShowings(string filePath)
        {
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                filePath = "../../../../MegaBios/obj/Debug/net8.0/" + filePath;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<RoomShowing>>(jsonString);
        }

        public static List<Account> LoadCustomers(string filePath)
        {
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                filePath = "../../../../MegaBios/obj/Debug/net8.0/" + filePath;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Account>>(jsonString);
        }

        // public static List<Guest> LoadGuests (string filePath) {
        //     string jsonString = File.ReadAllText(filePath);
        //     return JsonSerializer.Deserialize<List<Guest>>(jsonString);
        // }

        public static List<Guest> LoadGuests(string filePath)
        {
            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                filePath = "../../../../MegaBios/obj/Debug/net8.0/" + filePath;
            }
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

        // public static List<List<Seat>> GenerateSeating(int width, int height) {
        //     List<List<Seat>> seating = new List<List<Seat>>(height);
        //     for (int i = 0; i < height; i++) {
        //         seating.Add(new List<Seat>(width));
        //         for (int j = 1; j <= width; j++) {
        //             Seat seat = new Seat();
        //             seat.SeatNumber = $"{SeatSelect.rowLetters[i]}{j}";
        //             seat.SeatTaken = false;
        //             if (i == 0 && (j == 1 || j == 2 || j == 3 || j == width || j == width - 1 || j == width - 2)) {
        //                 seat.SeatType = "handicap";
        //             }
        //             else if (i != 0 && i % 2 != 0 && (j == 1 || j == 2 || j == width || j == width - 1)) {
        //                 seat.SeatType = "love seat";
        //             }
        //             else {
        //                 seat.SeatType = "normal";
        //             }
        //             seating[i].Add(seat);
        //         }
        //     }
        //     return seating;
        // }

        public static List<List<Seat>> GenerateSeating(int height, int width)
        {
            List<List<Seat>> seating = new List<List<Seat>>(height);

            for (int i = 0; i < height; i++)
            {
                seating.Add(new List<Seat>(height));

                for (int j = 1; j <= width; j++)
                {
                    Seat seat = new Seat();
                    seat.SeatNumber = $"{i}-{j}";
                    seat.SeatTaken = false;

                    if (i == 0 && (j == 1 || j == 2 || j == 3 || j == width || j == width - 1 || j == width - 2))
                    {
                        seat.SeatType = "handicap";
                        seat.Price = 10.00;
                    }
                    else if (i != 0 && i % 2 != 0 && (j == 1 || j == 2 || j == width || j == width - 1))
                    {
                        seat.SeatType = "love seat";
                        seat.Price = 20.00;
                    }
                    else
                    {
                        seat.SeatType = "normal";
                        seat.Price = 10.00;
                    }

                    seating[i].Add(seat);
                }
            }

            return seating;
        }
    }
}