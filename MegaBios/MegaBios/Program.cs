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
            int userChoice = -1;
            int cursorPos = 0;
            List<string> menuOptions = new() {"Ga verder als gast", "Creëer Account", "Login"};
            while(true) {
                Console.Clear();
                Console.WriteLine("Welkom bij MegaBios!");
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
                for (int i = 0; i < menuOptions.Count; i++) {
                    if (cursorPos == i) {
                        System.Console.WriteLine($"> {menuOptions[i]}");
                    }
                    else {
                        System.Console.WriteLine(menuOptions[i]);
                    }
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) {
                    userChoice = cursorPos + 1;
                    break;
                }
                // Select option for editing seating
                else if (keyInfo.Key == ConsoleKey.Tab) {
                    userChoice = 4;
                    break;
                }
                //Select Menu for creating/editing Cinemaroom json files
                else if (keyInfo.Key == ConsoleKey.LeftArrow) {
                    userChoice = 5;
                    break;
                }
                else {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
            }
            Console.Clear();
            switch (userChoice)
            {
                case 1:
                    LoginAsGuest();
                    break;
                case 2:
                    CreateAccount.CreateNewAccount(jsonData);
                    bool firstLogin = true;
                    while (true) {
                        (bool, TestAccount) loginInfo = Login(firstLogin);
                        if (loginInfo.Item1 && loginInfo.Item2 != null) {
                            LoggedInMenu(loginInfo.Item2);
                            break;
                        }
                        else {
                            firstLogin = false;
                        }
                    } 
                    break;                   
                case 3:
                    firstLogin = true;
                    while (true) {
                        (bool, TestAccount) loginInfo = Login(firstLogin);
                        if (loginInfo.Item1 && loginInfo.Item2 != null) {
                            LoggedInMenu(loginInfo.Item2);
                            break;
                        }
                        else {
                            firstLogin = false;
                        }
                    } 
                    break;
                
                case 4:
                    EditRoomSize();
                    break;

                case 5: 
                    CinemaRoomGenerator cinemaRoomGenerator = new CinemaRoomGenerator();
                    cinemaRoomGenerator.GenerationMenu();
                    break;
                default:
                    Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                    break;
            }
        }

        static void LoginAsGuest()
        {
            LoggedInAsGuest = true; // Setting LoggedInAsGuest to True
            movies = JsonFunctions.LoadMovies("../../../Movies.json");
            cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
            int cursorPos = 0;
            int userChoice = -1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
                List<string> menuOptions = new() {"Bestel ticket", "Maak een reservering"};
                for (int i = 0; i < menuOptions.Count; i++) {
                    if (cursorPos == i) {
                        System.Console.WriteLine($"> {menuOptions[i]}");
                    }
                    else {
                        System.Console.WriteLine(menuOptions[i]);
                    }
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) {
                    userChoice = cursorPos + 1;
                    break;
                }
                else {
                    cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                }
                // Console.WriteLine("1. Bestel ticket\n2. Maak een reservering\n");
            }

                switch (userChoice)
                {
                    case 1:
                        string movie = "Doornroosje";
                        List<CinemaRoom> cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
                        SeatSelect seatSelect = new(cinemaRooms[2]);
                        seatSelect.SelectSeats();
                        break;
                    case 2:
                        MakeReservation();
                        break;
                    default:
                        Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            
        }
        // Return type is tuple for successful login bool, account info
        static (bool, TestAccount) Login(bool firstLogin)
        {
            Console.Clear();
            if (!firstLogin) {
                Console.WriteLine("Invalide gebruikersnaam of wachtwoord. Probeer het alstublieft opnieuw.\n");
            }
            Console.WriteLine("Login Formulier");
            Console.WriteLine("-----------");

            Console.Write("Voer email in: ");
            string username = Console.ReadLine();

            Console.Write("Voer wachtwoord in: ");
            string password = HelperFunctions.MaskPasswordInput();

            foreach (TestAccount account in jsonData)
            {
                if (account.Email == username && account.Wachtwoord == password)
                {

                    Console.WriteLine("Succesvol ingelogd!");

                    movies = JsonFunctions.LoadMovies("../../../Movies.json");
                    cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");

                    return (true, account); 
                }
            }
            Console.WriteLine("Invalide gebruikersnaam of wachtwoord. Probeer het alstublieft opnieuw.");
            return (false, null);    
        }

        public static void LoggedInMenu(TestAccount account) {
            while(true) {
                int cursorPos = 0;
                int userChoice = -1;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Selecteer een optie met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
                    List<string> menuOptions = new() {"Toon Accountinformatie", "Verwijder Account", "Werk Accountinformatie Bij", "Bestel ticket", "Maak een reservering"};
                    // Console.WriteLine("1. Toon Accountinformatie \n2. Verwijder Account\n3. Werk Accountinformatie bij\n4. Bestel ticket\n5. Maak een reservering\n");
                    
                    for (int i = 0; i < menuOptions.Count; i++) {
                        if (cursorPos == i) {
                            System.Console.WriteLine($"> {menuOptions[i]}");
                        }
                        else {
                            System.Console.WriteLine(menuOptions[i]);
                        }
                    }
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter) {
                        userChoice = cursorPos + 1;
                        break;
                    }
                    else {
                        cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, menuOptions.Count);
                    }
                    
                }
                switch (userChoice)
                {
                    case 1:
                        Console.Clear();
                        ReadAccount.DisplayUserInfo(account);
                        System.Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                        ConsoleKeyInfo returnKeyPress = Console.ReadKey(true);

                        break;
                    case 2:
                        while (true)
                        {
                            System.Console.WriteLine("Weet u zeker dat u uw account wilt verwijderen? (ja/nee)\n");
                            string confirmInput = Console.ReadLine()!;
                            if (confirmInput == "ja")
                            {
                                DeleteAccount.RemoveAccount(jsonData, account);
                                return;
                            }
                            else if (confirmInput == "nee")
                            {
                                break;
                            }
                            else
                            {
                                System.Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                            }
                        }
                        break;
                    case 3:
                        UpdateAccount.UpdateField(account);
                        System.Console.WriteLine("Uw data is geupdatet!");
                        break;
                    case 4:
                        List<CinemaRoom> cinemaRooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
                        List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");\
                        MegaBiosData megaBiosData = JsonFunctions.LoadMegaBiosData("../../../MegaBiosData.json");
                        cursorPos = 0;
                        string selectedMovie = "";
                        while(true) {
                            Console.Clear();
                            Console.WriteLine("Selecteer een film met de pijltjestoetsen. Druk op Enter om je keuze te bevestigen\n");
                            for(int i = 0; i < movies.Count; i++) {
                                if (i == cursorPos) {
                                    System.Console.WriteLine($"> {movies[i].Title}");
                                }
                                else {
                                    System.Console.WriteLine($"{movies[i].Title}");
                                }
                            }
                            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Enter) {
                                selectedMovie = movies[cursorPos].Title;
                                break;
                            }
                            else {
                                cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, movies.Count);
                            }
                        }
                        while(true) {
                            Console.Clear();
                            System.Console.WriteLine("Voer in tussen welke twee tijdstippen je wilt kijken:\nformat is YY-mm-DD hh:mm:ss (Bijvoorbeeld: 2004-08-25 9:00:00))");
                            try {
                                DateTime timestamp1 = Convert.ToDateTime(Console.ReadLine());
                                DateTime timestamp2 = Convert.ToDateTime(Console.ReadLine());
                            }
                            catch (Exception ex) {
                                Console.WriteLine("" + ex);
                            }

                        }
                        
                        for (int i = 1; i <= megaBiosData.AmountOfRooms; i++) {
                            
                        }
                        
                        // for(int i = 0; i < cinemaRooms.Count; i++) {
                        //     Dictionary<string, MovieSchedule> roomSchedule = cinemaRooms[i].Schedule;
                        // }
                    
                        SeatSelect seatSelect = new(cinemaRooms[2]);
                        seatSelect.SelectSeats();
                        break;
                    case 5:
                        MakeReservation(account);
                        break;
                    default:
                        Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            }
            

        }

        public static void EditRoomSize() {
            int roomToEdit = -1;
            // Get the room number
            while (true) {
                Console.Clear();
                System.Console.WriteLine($"Welke zaal wil je updaten? Voer het nummer van de zaal in");
                roomToEdit = Convert.ToInt32(System.Console.ReadLine());
                if (File.Exists($"../../../Room{roomToEdit}.json")) {
                    break;
                }
            }
            // Get the width and height of the rooms and edit each seating plan in the json
            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../Room{roomToEdit}");
            for (int i = 0; i < roomShowings.Count; i++) {
                try {
                    while(true) {
                        System.Console.WriteLine("Hoe breed moet de zaal zijn?");
                        int roomWidth = Convert.ToInt32(Console.ReadLine());
                        System.Console.WriteLine("Hoe lang moet de zaal zijn?");
                        int roomHeight = Convert.ToInt32(Console.ReadLine());
                        List<List<Seat>> seating = JsonFunctions.GenerateSeating(roomWidth, roomHeight);
                        roomShowings[i].Seating = seating;
                        break;
                    }
                }
                catch (Exception e) {
                    System.Console.WriteLine("Voer alstublieft een valide nummer in");
                }
            } 
            JsonFunctions.WriteToJson($"../../../Room{roomToEdit}", roomShowings);
        }

        // public static void EditRoomSize() {
        //     List<CinemaRoom> rooms = JsonFunctions.LoadCinemaRooms("../../../CinemaRooms.json");
        //     while(true) {
        //         System.Console.WriteLine($"Welke zaal wil je updaten? Voer het nummer van de zaal in");
        //         for (int i = 0; i < rooms.Count; i++) {
        //             System.Console.WriteLine(rooms[i].RoomNumber);
        //         }
        //         try {
        //             int roomNumber = Convert.ToInt32(Console.ReadLine());
        //             if (0 < roomNumber - 1 && roomNumber - 1 < rooms.Count) {
        //                 while(true) {
        //                     try {
        //                         System.Console.WriteLine("Hoe breed moet de zaal zijn?");
        //                         int roomWidth = Convert.ToInt32(Console.ReadLine());
        //                         System.Console.WriteLine("Hoe lang moet de zaal zijn?");
        //                         int roomHeight = Convert.ToInt32(Console.ReadLine());
        //                         List<List<Seat>> seating = JsonFunctions.GenerateSeating(roomWidth, roomHeight);
        //                         rooms[roomNumber - 1].Seating = seating;
        //                         JsonFunctions.WriteToJson("../../../CinemaRooms.json", rooms);
        //                         break;
        //                     }
        //                     catch (Exception e) {
        //                         System.Console.WriteLine(e);
        //                         System.Console.WriteLine("Voer alsjeblieft een nummer in");
        //                     }
                            
        //                 }
        //                 break;
        //             }
        //         }
        //         catch (Exception e) {
        //             System.Console.WriteLine("Voer alstublieft een valide nummer in");
        //         }
        //     }    
        // }


        public static void MakeReservation()
        {
            string voornaam = "";
            string tussenvoegsel = "";
            string achternaam = "";
            string email = "";
            string telefoonNr = "";
            bool is_student = false;

            System.Console.WriteLine("Selecteer een film om een reservatie te maken:");
            for (int i = 0; i < movies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {movies[i].Title}");
            }
            System.Console.WriteLine("Voer het nummer van de film in die u wilt reserveren: ");
            int selectedMovieIndex = Convert.ToInt32(Console.ReadLine()) - 1;

            if (selectedMovieIndex < 0 || selectedMovieIndex >= movies.Count)
            {
                Console.WriteLine("Invalide selectie.");
                return;
            }

            // Ask information only to Guest userrs
            if (LoggedInAsGuest == true)
            {
                Console.Write("Voer voornaam in: ");
                voornaam = Console.ReadLine();

                Console.Write("Voer tussenvoegsel in (als u een tussenvoegsel heeft): ");
                tussenvoegsel = Console.ReadLine();

                Console.Write("Voer achternaam in: ");
                achternaam = Console.ReadLine();

                Console.Write("Voer email in: ");
                email = Console.ReadLine()!;

                Console.Write("Voer telefoonnummer in: ");
                telefoonNr = Console.ReadLine()!;

                while (true)
                {
                    Console.Write("Bent u student? (true/false): ");
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
                Console.WriteLine($"Succesvol een reservering gemaakt voor '{selectedMovie.Title}'. Uw reserveringnummer is {reservationNumber}.\n");
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
                Console.WriteLine($"Succesvol een reservering gemaakt voor '{selectedMovie.Title}'. Uw reserveringnummer is {reservationNumber}.");
            }
        }

        public static void MakeReservation(TestAccount user)
        {
            Console.WriteLine("Selecteer een film om een reservatie te maken:");
            for (int i = 0; i < movies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {movies[i].Title}");
            }
            Console.Write("Voer het nummer van de film in die u wilt reserveren: ");
            int selectedMovieIndex = Convert.ToInt32(Console.ReadLine()) - 1;

            if (selectedMovieIndex < 0 || selectedMovieIndex >= movies.Count)
            {
                Console.WriteLine("Invalide selectie.");
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

            Console.WriteLine($"Succesvol een reservering gemaakt voor '{selectedMovie.Title}'. Uw reserveringnummer is {reservationNumber}.");
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
