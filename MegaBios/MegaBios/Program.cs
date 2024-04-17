using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MegaBios
{
    class Program
    {
        public static List<Movie> movies = new List<Movie>();
        public static List<CinemaRoom> cinemaRooms = new List<CinemaRoom>();
        public static List<TestAccount> jsonData = new List<TestAccount>();
        public static string jsonFilePath = "../../../customers.json";
        public static bool LoggedInAsGuest = false;

        static void Main(string[] args)
        {
            // JSON bestand ophalen
            string jsonText = File.ReadAllText(jsonFilePath);
            JsonDocument jsonDocument = JsonDocument.Parse(jsonText);
            JsonElement root = jsonDocument.RootElement;
            jsonData = JsonFunctions.ConvertJsonToList(root);
            string choice = "";

            Console.WriteLine("Welcome to MegaBios!");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Ga verder als gast");
            Console.WriteLine("2. Create Account");
            Console.WriteLine("3. Login");

            while (choice != "1" && choice != "2" && choice != "3")
            {

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LoginAsGuest();
                        break;
                    case "2":
                        CreateAccount.CreateNewAccount(jsonData);
                        Login();
                        break;
                    case "3":
                        Login();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void LoginAsGuest()
        {
            LoggedInAsGuest = true; // Setting LoggedInAsGuest to True
            movies = JsonFunctions.LoadMovies("../../../Movies.json");
            cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
            while (true)
            {
                Console.WriteLine("1. Order ticket\n2. Make a Reservation\n");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        string movie = "Doornroosje";
                        List<CinemaRoom> cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
                        SeatSelect seatSelect = new(cinemaRooms[0]);
                        seatSelect.SelectSeats();
                        break;
                    case "2":
                        MakeReservation();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
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

        public static void MakeReservation()
        {
            string voornaam = "";
            string tussenvoegsel = "";
            string achternaam = "";
            string email = "";
            string telefoonNr = "";
            bool is_student = false;

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

            // Ask information only to Guest userrs
            if (LoggedInAsGuest == true)
            {
                Console.Write("Enter first name: ");
                voornaam = Console.ReadLine();

                Console.Write("Enter middle name (if any): ");
                tussenvoegsel = Console.ReadLine();

                Console.Write("Enter last name: ");
                achternaam = Console.ReadLine();

                Console.Write("Enter email: ");
                email = Console.ReadLine()!;

                Console.Write("Enter phone number: ");
                telefoonNr = Console.ReadLine()!;

                while (true)
                {
                    Console.Write("Are you a student? (true/false): ");
                    string is_studentString = Console.ReadLine()!;

                    if (is_studentString == "true" || is_studentString == "false")
                    {
                        is_student = Convert.ToBoolean(is_studentString);
                        break;
                    }
                }
            }

            var selectedMovie = movies[selectedMovieIndex];
            var reservationNumber = Guid.NewGuid().ToString(); // GUID voor uniek reserveringsnummer
            var reservation = new ReservationHistory
            {
                ReservationNumber = reservationNumber,
                MovieTitle = selectedMovie.Title,
                ReservationDate = DateTime.Now
            };

            // Voeg de reservering toe aan een gast-account (hier wordt geen specifiek account vereist)
            JsonFunctions.WriteToJson(jsonFilePath, jsonData);

            if (LoggedInAsGuest == true)
            {
                Console.WriteLine($"Reservation made successfully for '{selectedMovie.Title}'. Your reservation number is {reservationNumber}.\n");
                Console.WriteLine("Bestelling Overzicht:\n");
                if (tussenvoegsel != "")
                {
                    Console.WriteLine($"Naam: {voornaam} {tussenvoegsel} {achternaam}");
                }
                else
                {
                    Console.WriteLine($"Naam: {voornaam} {achternaam}");
                }
                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"Telefoonnummer: {telefoonNr}");
                if (is_student)
                {
                    Console.WriteLine("Studentenkorting is toegepast!\n");
                }

            }
            else
            {
                Console.WriteLine($"Reservation made successfully for '{selectedMovie.Title}'. Your reservation number is {reservationNumber}.");
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
            Console.WriteLine("Bestelling Overzicht:\n");
            if (user.Tussenvoegsel != "")
            {
                Console.WriteLine($"Naam: {user.Voornaam} {user.Tussenvoegsel} {user.Achternaam}");
            }
            else
            {
                Console.WriteLine($"Naam: {user.Voornaam} {user.Achternaam}");
            }
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Telefoonnummer: {user.TelefoonNr}");
            if (user.IsStudent)
            {
                Console.WriteLine("Studentenkorting is toegepast!\n");
            }
        }
    }
}
