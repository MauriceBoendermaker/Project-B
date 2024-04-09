using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    class Program
    {
        public static List<Movie> movies = new();
        public static List<CinemaRoom> cinemaRooms = new();
        public static List<TestAccount> jsonData = new();
        public static string jsonFilePath = "../../../customers.json";

        static void Main(string[] args)
        {
            // JSON bestand ophalen

            string jsonText = File.ReadAllText(jsonFilePath);

            JsonDocument jsonDocument = JsonDocument.Parse(jsonText);
            JsonElement root = jsonDocument.RootElement;
            jsonData = JsonFunctions.ConvertJsonToList(root);

            Console.WriteLine("Welcome to MegaBios!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Login");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateAccount.CreateNewAccount(jsonData);
                    Login();
                    break;
                case 2:
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            static void Login()
            {
                Console.WriteLine("Login Form");
                Console.WriteLine("-----------");

                Console.Write("Enter email: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string password = HelperFunctions.MaskPasswordInput();

                bool isAuthenticated = false;

                foreach (TestAccount account in jsonData)
                {
                    if (account.Email == username && account.Wachtwoord == password)
                    {
                        isAuthenticated = true;

                        Console.WriteLine("Login successful!");

                        movies = JsonFunctions.LoadMovies("../../../Movies.json");
                        cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");

                        bool isLoggedIn = true;
                        while (true)
                        {

                            Console.WriteLine("1. Display Account Information \n2. Delete Account\n3. Update Account Information\n4. Order ticket\n5. Make a Reservation\n");
                            string userChoice = Console.ReadLine();

                            switch (userChoice)
                            {
                                case "1":
                                    ReadAccount.DisplayUserInfo(account);
                                    break;
                                case "2":
                                    while (true)
                                    {
                                        System.Console.WriteLine("Are you sure you want to delete your account? (yes/no)\n");
                                        string confirmInput = Console.ReadLine()!;
                                        if (confirmInput == "yes")
                                        {
                                            DeleteAccount.RemoveAccount(jsonData, account);
                                            isLoggedIn = false;
                                            break;
                                        }
                                        else if (confirmInput == "no")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            System.Console.WriteLine("Invalid input.");
                                        }
                                    }
                                    break;
                                case "3":
                                    UpdateAccount.UpdateField(account);
                                    System.Console.WriteLine("Successfully updated your data!");
                                    break;
                                case "4":
                                    string movie = "Doornroosje";
                                    List<CinemaRoom> cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");                     
                                    SeatSelect seatSelect = new(cinemaRooms[0]);
                                    seatSelect.SelectSeats();
                                    break;
                                case "5":
                                    MakeReservation(account);
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                            if (!isLoggedIn)
                            {
                                break;
                            }
                        }
                        break;
                    }
                }

                if (!isAuthenticated)
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                    // TODO: Code toevoegen voor verkeerde login pogingen
                }
            }

            //CreateAccount.CreateNewAccount(jsonData);
        }

        public static void ListMovies()
        {
            foreach (var movie in movies)
            {
                Console.WriteLine($"Title: {movie.Title}, Genre: {movie.Genre}, Rating: {movie.Rating}");
            }
        }

        public static void MakeReservation(TestAccount user)
        {
            Console.WriteLine("Select a movie to make a reservation:");
            for (int i = 0; i < movies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {movies[i].Title}");
            }
            Console.Write("Enter the number of the movie you want to reserve: ");
            int selectedMovieIndex = Convert.ToInt32(Console.ReadLine()) - 1;

            if (selectedMovieIndex < 0 || selectedMovieIndex >= movies.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedMovie = movies[selectedMovieIndex];
            var reservationNumber = Guid.NewGuid().ToString(); // GUID voor uniek reserverings nummer
            var reservation = new ReservationHistory
            {
                ReservationNumber = reservationNumber,
                MovieTitle = selectedMovie.Title,
                ReservationDate = DateTime.Now
            };

            user.History.Add(reservation);

            JsonFunctions.WriteToJson(jsonFilePath, jsonData);

            Console.WriteLine($"Reservation made successfully for '{selectedMovie.Title}'. Your reservation number is {reservationNumber}.");
        }
    }
}