using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

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
            jsonData = JsonFunctions.LoadCustomers("../../../customers.json");
            int cursorPos = 0;
            List<string> menuOptions = new() { "Ga verder als gast", "Creëer Account", "Login", "Admin1", "Admin2" };
            int userChoice = MenuFunctions.Menu(menuOptions) + 1;
            Console.Clear();
            switch (userChoice)
            {
                case 1:
                    LoginAsGuest();
                    break;
                case 2:
                    CreateAccount.CreateNewAccount(jsonData);
                    bool firstLogin = true;
                    while (true)
                    {
                        (bool, TestAccount) loginInfo = Login(firstLogin);
                        if (loginInfo.Item1 && loginInfo.Item2 != null)
                        {
                            LoggedInMenu(loginInfo.Item2);
                            break;
                        }
                        else
                        {
                            firstLogin = false;
                        }
                    }
                    break;
                case 3:
                    firstLogin = true;
                    while (true)
                    {
                        (bool, TestAccount) loginInfo = Login(firstLogin);
                        if (loginInfo.Item1 && loginInfo.Item2 != null)
                        {
                            LoggedInMenu(loginInfo.Item2);
                            break;
                        }
                        else
                        {
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
            List<string> menuOptions = new() { "Bestel ticket", "Annuleer reserveringen"};
            StringBuilder sb = new StringBuilder();
            sb.Append("Welkom bij MegaBios!");
            userChoice = MenuFunctions.Menu(menuOptions, sb) + 1;
            switch (userChoice)
            {
                // Bestel ticket
                case 1:
                    ReservationHistory reservation = TicketReservation();
                    Guest guest = Guest.CreateGuest();
                    reservation.ReservedSeats = ReservationHistory.ApplyDiscount(reservation.ReservedSeats, guest);
                    bool confirmedPayment = ReservationHistory.ConfirmPayment(reservation);
                    if (confirmedPayment)
                    {
                        SeatSelect.MarkSeatsAsSelected(reservation.ReservedSeats, reservation.ReservationDate, reservation.ReservationRoom);
                        ReservationHistory.AddReservation(guest, reservation);
                    }
                    break;
                // Annuleer tickets
                case 2:
                    guest = Guest.CreateGuest();
                    if (guest.History.Count > 0) {
                        CancelMenu(guest);
                    }
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
            if (!firstLogin)
            {
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

        public static void LoggedInMenu(TestAccount account)
        {
            while (true)
            {
                List<string> menuOptions = new() { "Toon Accountinformatie", "Verwijder Account", "Werk Accountinformatie Bij", "Bestel ticket", "Annuleer ticket(s)", "Bestellingen" };
                int cursorPos = 0;
                int userChoice = MenuFunctions.Menu(menuOptions) + 1;

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
                        ReservationHistory reservation = TicketReservation();
                        reservation.ReservedSeats = ReservationHistory.ApplyDiscount(reservation.ReservedSeats, account);
                        bool confirmedPayment = ReservationHistory.ConfirmPayment(reservation);
                        if (confirmedPayment)
                        {
                            SeatSelect.MarkSeatsAsSelected(reservation.ReservedSeats, reservation.ReservationDate, reservation.ReservationRoom);
                            ReservationHistory.AddReservation(account, reservation);
                        }
                        break;


                    case 5:
                        CancelMenu(account);
                        break;
                    case 6:
                        Console.Clear();

                        foreach (var userReservation in account.History)
                        {
                            Console.WriteLine(ReservationHistory.PrintReservationUser(userReservation));
                        }
                        Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                        Console.ReadKey(true);
                        break;
                    default:
                        Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            }


        }

        public static void EditRoomSize()
        {
            int roomToEdit = -1;
            // Get the room number
            while (true)
            {
                Console.Clear();
                System.Console.WriteLine($"Welke zaal wil je updaten? Voer het nummer van de zaal in");
                roomToEdit = Convert.ToInt32(System.Console.ReadLine());
                if (File.Exists($"../../../Room{roomToEdit}.json"))
                {
                    break;
                }
            }
            // Get the width and height of the rooms and edit each seating plan in the json
            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../Room{roomToEdit}");
            for (int i = 0; i < roomShowings.Count; i++)
            {
                try
                {
                    while (true)
                    {
                        System.Console.WriteLine("Hoe breed moet de zaal zijn?");
                        int roomWidth = Convert.ToInt32(Console.ReadLine());
                        System.Console.WriteLine("Hoe lang moet de zaal zijn?");
                        int roomHeight = Convert.ToInt32(Console.ReadLine());
                        List<List<Seat>> seating = JsonFunctions.GenerateSeating(roomWidth, roomHeight);
                        roomShowings[i].Seating = seating;
                        break;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Voer alstublieft een valide nummer in");
                }
            }
            JsonFunctions.WriteToJson($"../../../Room{roomToEdit}", roomShowings);
        }

        public static ReservationHistory TicketReservation()
        {
            List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");

            // Creates list of movie titles from movies
            List<string> movieTitles = movies.Select(o => o.Title).ToList();

            int cursorPos = MenuFunctions.Menu(movieTitles);
            string selectedMovie = movies[cursorPos].Title;
            Dictionary<string, DateTime> showingOptions;
            string selectedRoom = "";
            DateTime initialDate;
            DateTime selectedDate;
            bool redirectedDate = false;

            while (true)
            {
                Console.Clear();
                System.Console.WriteLine("Selecteer een dag met de pijltjestoetsen, druk op Enter om je selectie te bevestigen");
                List<DateTime> menuOptions = GetShowDays();
                int selectedOption = MenuFunctions.Menu(menuOptions, false);
                selectedDate = menuOptions[selectedOption];

                initialDate = selectedDate;

                showingOptions = GetShowingOptions(selectedDate, selectedMovie);

                if (showingOptions.Count == 0)
                {
                    while (showingOptions.Count == 0)
                    {
                        redirectedDate = true;
                        selectedDate = selectedDate.AddDays(1);
                        showingOptions = GetShowingOptions(selectedDate, selectedMovie);
                    }
                }
                break;
            }

            cursorPos = 0;

            Console.Clear();
            List<string> keys = showingOptions.Keys.ToList();
            if (redirectedDate)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"Er zijn geen tentoonstellingen beschikbaar voor {selectedMovie} op {initialDate.Date}\nOp {selectedDate.Date} zijn er wel films beschikbaar. Hierbij de films van die dag:");
                cursorPos = MenuFunctions.Menu(keys, sb);
            }
            else
            {
                cursorPos = MenuFunctions.Menu(keys);
            }

            selectedRoom = keys[cursorPos].Split(" - ")[0].Replace(" ", "");
            selectedDate = showingOptions[keys[cursorPos]];

            System.Console.WriteLine($"../../../{selectedRoom}.json");
            List<RoomShowing> selectedShowing = JsonFunctions.LoadRoomShowings($"../../../{selectedRoom}.json");

            // Prompt user to choose between individual or group seat selection
            Console.Clear();
            List<string> selectionOptions = new List<string> { "Individueel", "Als groep" };
            cursorPos = 0;
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < selectionOptions.Count; i++)
                {
                    if (cursorPos == i)
                    {
                        Console.WriteLine($"\x1b[42m{selectionOptions[i]}\x1b[0m");
                    }
                    else
                    {
                        Console.WriteLine(selectionOptions[i]);
                    }
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                cursorPos = MenuFunctions.MoveCursor(cursorPos, keyInfo, selectionOptions.Count);
            }

            SeatSelect seatSelect = new(selectedShowing, selectedRoom, selectedDate);
            List<Seat> selectedSeats;
            if (cursorPos == 0)
            {
                selectedSeats = seatSelect.SelectSeats();
                string reservationNumber = ReservationHistory.generateReservationNumber();
                ReservationHistory reservation = new(reservationNumber, selectedMovie, selectedSeats, selectedRoom, selectedDate);
                return reservation;
            }
            else if (cursorPos == 1)
            {
                Console.WriteLine("Hoeveel personen zijn er in de groep?");
                int groupSize = int.Parse(Console.ReadLine());
                selectedSeats = seatSelect.SelectGroupSeats(groupSize);
                string reservationNumber = ReservationHistory.generateReservationNumber();
                ReservationHistory reservation = new(reservationNumber, selectedMovie, selectedSeats, selectedRoom, selectedDate);
                return reservation;
            }
            return null;

            // else
            // {
            //     Console.WriteLine("Ongeldige selectie. Probeer het opnieuw.");
            //     TicketReservation(); // Restart the reservation process if invalid option is selected
            //     return (null, null, DateTime.MinValue); // Ensure to return after restarting to avoid continuation of current flow
            // }

        }
        public static Dictionary<string, DateTime> GetShowingOptions(DateTime date, string selectedMovie)
        {
            Dictionary<string, DateTime> showingOptions = new Dictionary<string, DateTime>();
            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++)
            {
                List<RoomShowing> showings = JsonFunctions.LoadRoomShowings($"../../../Room{i}.json");
                for (int j = 0; j < showings.Count; j++)
                {
                    // If the movie title is equal and the showing time is in between given timestamps
                    // if (showings[i].Movie == selectedMovie && timestamp1 < showings[i].ShowingTime && showings[j].ShowingTime < timestamp2) {
                    if (showings[j].Movie == selectedMovie && date.Date == showings[j].ShowingTime.Date && !SeatSelect.IsFull(showings[i].Seating))
                    {
                        showingOptions.Add($"Room {i} - {showings[j].ShowingTime}", showings[j].ShowingTime);
                    }
                }
            }
            return showingOptions;
        }

        public static List<DateTime> GetShowDays()
        {
            List<DateTime> showDays = new();
            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../Room1.json");
            DateTime startDate = roomShowings[0].ShowingTime.Date;
            for (int i = 0; i < 7; i++)
            {
                if (startDate.AddDays(i) > DateTime.Now)
                {
                    showDays.Add(startDate.AddDays(i));
                }
            }
            return showDays;
        }

        public static ReservationHistory CancelSeat(TestAccount account, ReservationHistory reservation, Seat seat) {
            Console.Clear();
            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> {"Ja", "Nee"}, sb);
            switch (selectedOption) {
                // Ja
                case 0:
                    reservation.ReservedSeats.Remove(seat);
                    System.Console.WriteLine($"U heeft {seat.Price} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);
                    return reservation;
                //Nee
                case 1:
                    return reservation;
            }
            return reservation;
        }

        public static TestAccount CancelReservation(TestAccount account, ReservationHistory reservation, List<Seat> seats) {
            Console.Clear();
            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> {"Ja", "Nee"}, sb);
            switch (selectedOption) {
                // Ja
                case 0:
                    account.History.Remove(reservation);
                    double totalPrice = 0;
                    foreach(Seat seat in seats) {
                        totalPrice += seat.Price;
                    }
                    System.Console.WriteLine($"U heeft {totalPrice} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);
                    return account;

                //Nee
                case 1:
                    return account;
            }
            return account;
        }

        public static ReservationHistory CancelSeat(Guest guest, ReservationHistory reservation, Seat seat) {
            Console.Clear();
            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> {"Ja", "Nee"}, sb);
            switch (selectedOption) {
                // Ja
                case 0:
                    reservation.ReservedSeats.Remove(seat);
                    System.Console.WriteLine($"U heeft {seat.Price} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);
                    return reservation;
                //Nee
                case 1:
                    return reservation;
            }
            return reservation;
        }

        public static Guest CancelReservation(Guest guest, ReservationHistory reservation, List<Seat> seats) {
            Console.Clear();
            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> {"Ja", "Nee"}, sb);
            switch (selectedOption) {
                // Ja
                case 0:
                    guest.History.Remove(reservation);
                    double totalPrice = 0;
                    foreach(Seat seat in seats) {
                        totalPrice += seat.Price;
                    }
                    System.Console.WriteLine($"U heeft {totalPrice} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);
                    return guest;

                //Nee
                default:
                    return guest;
            }
        }

        public static void CancelMenu(TestAccount account) {
            List<string> reservationNumbers = account.History
                .Select(x => x.ReservationNumber)
                .ToList();
            int selectedOption = MenuFunctions.Menu(reservationNumbers);
            string reservationNumber = reservationNumbers[selectedOption];
            ReservationHistory selectedReservation = account.History
                .Where(x => x.ReservationNumber == reservationNumber)
                .ToList()[0];
            List<string> menuOptions = new List<string>() {"Annuleer 1 stoel", "Annuleer hele reservering"};
            
            // Ask user to select whether they want to cancel 1 seat or all seats
            StringBuilder sb = new StringBuilder();
            sb.Append("Wat voor annulering wilt u uitvoeren?");
            selectedOption = MenuFunctions.Menu(menuOptions, sb);
            switch (selectedOption) {
                // Cancel 1 seat
                case 0: 
                    List<string> seatNumbers = selectedReservation.ReservedSeats
                        .Select(x => x.SeatNumber)
                        .ToList();
                    sb = new StringBuilder();
                    sb.Append("Welke stoel wilt u annuleren?");
                    selectedOption = MenuFunctions.Menu(seatNumbers, sb);
                    
                    // Get the correct seat from the seats list
                    Seat selectedSeat = selectedReservation.ReservedSeats
                        .Where(x => x.SeatNumber == seatNumbers[selectedOption])
                        .ToList()[0];                                  
                    ReservationHistory changedReservation = CancelSeat(account, selectedReservation, selectedSeat);
                    for (int i = 0; i < account.History.Count; i++) {
                        if (account.History[i].ReservationNumber == changedReservation.ReservationNumber) {
                            account.History[i] = changedReservation;
                            break;
                        }
                    }
                    break;
                // Cancel whole reservation
                case 1:
                    account = CancelReservation(account, selectedReservation, selectedReservation.ReservedSeats);
                    break;
            }
            TestAccount.UpdateAccount(account);
        }

        public static void CancelMenu(Guest guest) {
            List<string> reservationNumbers = guest.History
                .Select(x => x.ReservationNumber)
                .ToList();
            int selectedOption = MenuFunctions.Menu(reservationNumbers);
            string reservationNumber = reservationNumbers[selectedOption];
            ReservationHistory selectedReservation = guest.History
                .Where(x => x.ReservationNumber == reservationNumber)
                .ToList()[0];
            List<string> menuOptions = new List<string>() {"Annuleer 1 stoel", "Annuleer hele reservering"};
            
            // Ask user to select whether they want to cancel 1 seat or all seats
            StringBuilder sb = new StringBuilder();
            sb.Append("Wat voor annulering wilt u uitvoeren?");
            selectedOption = MenuFunctions.Menu(menuOptions, sb);
            switch (selectedOption) {
                // Cancel 1 seat
                case 0: 
                    List<string> seatNumbers = selectedReservation.ReservedSeats
                        .Select(x => x.SeatNumber)
                        .ToList();
                    sb = new StringBuilder();
                    sb.Append("Welke stoel wilt u annuleren?");
                    selectedOption = MenuFunctions.Menu(seatNumbers, sb);
                    
                    // Get the correct seat from the seats list
                    Seat selectedSeat = selectedReservation.ReservedSeats
                        .Where(x => x.SeatNumber == seatNumbers[selectedOption])
                        .ToList()[0];                                  
                    ReservationHistory changedReservation = CancelSeat(guest, selectedReservation, selectedSeat);
                    for (int i = 0; i < guest.History.Count; i++) {
                        if (guest.History[i].ReservationNumber == changedReservation.ReservationNumber) {
                            guest.History[i] = changedReservation;
                            break;
                        }
                    }
                    break;
                // Cancel whole reservation
                case 1:
                    guest = CancelReservation(guest, selectedReservation, selectedReservation.ReservedSeats);
                    break;
            }
            Guest.UpdateAccount(guest);
        }
    }
}
