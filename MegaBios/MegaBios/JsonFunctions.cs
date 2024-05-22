using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MegaBios
{
    internal class JsonFunctions
    {
        public static void WriteToJson<T>(string filePath, T data)
        {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }

        public static TestAccount ConvertJsonToAccount(JsonElement user)
        {
            string voornaam = user.GetProperty("voornaam").GetString()!;
            string tussenvoegsel = user.GetProperty("tussenvoegsel").GetString()!;
            string achternaam = user.GetProperty("achternaam").GetString()!;
            string geboorteDatum = user.GetProperty("geboorte_datum").GetString()!;

            Dictionary<string, string> adres = new();
            JsonElement adresElement = user.GetProperty("adres")!;

            foreach (var property in adresElement.EnumerateObject())
            {
                adres.Add(property.Name, property.Value.GetString()!);
            }

            string email = user.GetProperty("email").GetString()!;
            string wachtwoord = user.GetProperty("wachtwoord").GetString()!;
            string telefoonNr = user.GetProperty("telefoonnummer").GetString()!;
            string betaalwijze = user.GetProperty("voorkeur_betaalwijze").GetString()!;
            bool is_student = user.GetProperty("is_student").GetBoolean()!;

            return new TestAccount(voornaam, tussenvoegsel, achternaam, geboorteDatum, adres, email, wachtwoord, telefoonNr, betaalwijze, is_student);
        }

        public static List<TestAccount> ConvertJsonToList(JsonElement root)
        {
            foreach (JsonElement user in root.EnumerateArray())
            {
                Program.jsonData.Add(ConvertJsonToAccount(user));
            }
            return Program.jsonData;
        }

        public static List<Movie> LoadMovies(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Movie>>(jsonString);
        }

        public static List<RoomShowing> LoadRoomShowings (string filePath) {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<RoomShowing>>(jsonString);
        }

        public static List<TestAccount> LoadCustomers (string filePath) {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<TestAccount>>(jsonString);
        }

        public static List<CinemaRoom> LoadCinemaRooms(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                CinemaData cinemaData = JsonSerializer.Deserialize<CinemaData>(jsonString, options);
                return cinemaData?.CinemaRooms ?? new List<CinemaRoom>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize JSON to type 'CinemaData': {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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

        public static List<List<Seat>> GenerateSeating(int width, int height) {
            List<List<Seat>> seating = new List<List<Seat>>(height);
            for (int i = 0; i < height; i++) {
                seating.Add(new List<Seat>(width));
                for (int j = 1; j <= width; j++) {
                    Seat seat = new Seat();
                    seat.SeatNumber = $"{SeatSelect.rowLetters[i]}{j}";
                    seat.SeatTaken = false;
                    if (i == 0 && (j == 1 || j == 2 || j == 3 || j == width || j == width - 1 || j == width - 2)) {
                        seat.SeatType = "handicap";
                        seat.Price = 10.00;
                    }
                    else if (i != 0 && i % 2 != 0 && (j == 1 || j == 2 || j == width || j == width - 1)) {
                        seat.SeatType = "love seat";
                        seat.Price = 20.00;
                    }
                    else {
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
