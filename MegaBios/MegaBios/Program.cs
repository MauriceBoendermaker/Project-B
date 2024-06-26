﻿using System.Text;
using System.Text.Json;

namespace MegaBios
{
    public class Program
    {
        public static List<Movie> movies = new List<Movie>();
        public static List<Account> jsonData = new List<Account>();
        public static bool LoggedInAsGuest = false;

        static void Main(string[] args)
        {
            // JSON bestand ophalen
            string jsonText = File.ReadAllText("../../../customers.json");
            JsonDocument jsonDocument = JsonDocument.Parse(jsonText);
            JsonElement root = jsonDocument.RootElement;
            jsonData = JsonFunctions.LoadCustomers("../../../customers.json");

            while (true)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Welkom bij MegaBios!");
                List<string> menuOptions = new() { "Ga verder als gast", "Creëer Account", "Login" };
                int userChoice = MenuFunctions.Menu(menuOptions, sb, false) + 1;

                Console.Clear();

                switch (userChoice)
                {
                    case 1:
                        LoginAsGuest();
                        break;
                    case 2:
                        CreateAccount.CreateNewAccount(jsonData);
                        bool firstLogin = true;
                        int attempts = 0;

                        while (true)
                        {
                            if (attempts >= 3)
                            {
                                Console.Clear();
                                Console.WriteLine("U heeft 3 foute inlogpogingen gedaan. Druk op een willekeurige toets om terug te keren naar het menu");
                                Console.ReadKey(true);
                                break;
                            }
                            (bool, Account) loginInfo = Login(firstLogin);

                            if (loginInfo.Item1 && loginInfo.Item2 != null)
                            {
                                LoggedInMenu(loginInfo.Item2);
                                break;
                            }
                            else
                            {
                                firstLogin = false;
                                attempts++;
                            }
                        }
                        break;
                    case 3:
                        firstLogin = true;
                        attempts = 0;

                        while (true)
                        {
                            if (attempts >= 3)
                            {
                                Console.Clear();
                                Console.WriteLine("U heeft 3 foute inlogpogingen gedaan. Druk op een willekeurige toets om terug te keren naar het menu");
                                Console.ReadKey(true);
                                break;
                            }
                            (bool, Account) loginInfo = Login(firstLogin);

                            if (loginInfo.Item1 && loginInfo.Item2 != null)
                            {
                                LoggedInMenu(loginInfo.Item2);
                                break;
                            }
                            else
                            {
                                firstLogin = false;
                                attempts++;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            }
        }

        static void LoginAsGuest()
        {
            while (true)
            {
                LoggedInAsGuest = true; // Zet LoggedInAsGuest naar True
                movies = JsonFunctions.LoadMovies("../../../Movies.json");
                jsonData = JsonFunctions.LoadCustomers("../../../customers.json");

                int userChoice = -1;

                List<string> menuOptions = new() { "Bestel ticket", "Annuleer reserveringen" };
                StringBuilder sb = new StringBuilder();

                sb.Append("Welkom bij MegaBios!");
                userChoice = MenuFunctions.Menu(menuOptions, sb) + 1;

                switch (userChoice)
                {
                    case 0:
                        return;
                    // Bestel ticket
                    case 1:
                        Reservation reservation = TicketReservation();

                        if (reservation == null)
                        {
                            break;
                        }

                        Console.ReadKey(true);
                        Guest guest = Guest.CreateGuest();

                        bool confirmedPayment = Reservation.ConfirmPayment(reservation);

                        if (confirmedPayment)
                        {
                            SeatSelect.MarkSeatsAsSelected(reservation.ReservedSeats, reservation.ShowingDate, reservation.ReservationRoom);
                            Reservation.AddReservation(guest, reservation);
                        }

                        break;
                    // Annuleer tickets
                    case 2:
                        guest = Guest.CreateGuest();

                        if (guest.Reservations.Count > 0)
                        {
                            CancelMenu(guest);
                        }

                        break;
                    // Toon reserveringen voor account
                    case 3:
                        guest = Guest.CreateGuest();

                        if (guest.Reservations.Count > 0)
                        {
                            Console.Clear();

                            foreach (var userReservation in guest.Reservations)
                            {
                                Console.WriteLine(Reservation.PrintReservationUser(userReservation));
                            }

                            Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                            Console.ReadKey(true);

                            break;
                        }
                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            }
        }

        // Return type is Tuple voor succesvolle login Bool, account info
        static (bool, Account) Login(bool firstLogin)
        {
            Console.Clear();

            if (!firstLogin)
            {
                Console.WriteLine("Ongeldige gebruikersnaam of wachtwoord. Probeer het alstublieft opnieuw.\n");
            }

            Console.WriteLine("Login Formulier");
            Console.WriteLine("Druk enter om je input te bevestigen");
            Console.WriteLine("-----------");

            Console.Write("Voer email in: ");
            string username = Console.ReadLine();

            Console.Write("Voer wachtwoord in: ");
            string password = HelperFunctions.MaskPasswordInput();

            foreach (Account account in jsonData)
            {
                if (account.Email == username && account.Wachtwoord == password)
                {
                    Console.WriteLine("Succesvol ingelogd!");

                    movies = JsonFunctions.LoadMovies("../../../Movies.json");

                    return (true, account);
                }
            }

            Console.WriteLine("Ongeldige gebruikersnaam of wachtwoord. Probeer het alstublieft opnieuw.");
            return (false, null);

        }

        public static void LoggedInMenu(Account account)
        {
            while (true)
            {
                account = account.ReloadAccount();
                List<string> menuOptions = new() { "Toon accountinformatie", "Verwijder account", "Werk accountinformatie bij", "Bestel ticket", "Annuleer ticket(s)", "Toon reserveringen", "Toon bestelgeschiedenis" };

                if (account.IsAdmin())
                {
                    menuOptions.Add("Pas zaal grootte aan");
                    menuOptions.Add("Admin methods");
                }

                int userChoice = MenuFunctions.Menu(menuOptions) + 1;

                switch (userChoice)
                {
                    case 0:
                        return;
                    case 1:
                        Console.Clear();
                        ReadAccount readAccount = new();
                        readAccount.DisplayUserInfo(account);
                        Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                        ConsoleKeyInfo returnKeyPress = Console.ReadKey(true);

                        break;
                    case 2:
                        while (true)
                        {
                            Console.WriteLine("Weet u zeker dat u uw account wilt verwijderen? (ja/nee)\n");
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
                                Console.WriteLine("Invalide keuze. Probeer het alstublieft opnieuw.");
                            }
                        }
                        break;
                    case 3:
                        UpdateAccount.UpdateField(account);
                        Console.WriteLine("Uw data is geupdatet!");

                        break;
                    case 4:
                        Reservation reservation = TicketReservation(account);

                        if (reservation == null)
                        {
                            break;
                        }

                        bool confirmedPayment = Reservation.ConfirmPayment(reservation);

                        if (confirmedPayment)
                        {
                            SeatSelect.MarkSeatsAsSelected(reservation.ReservedSeats, reservation.ShowingDate, reservation.ReservationRoom);
                            Reservation.AddReservation(account, reservation);
                        }

                        break;
                    case 5:
                        CancelMenu(account);
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine($"--------UW RESERVERINGEN-----------\n");
                        Reservation[] reservations = account.Reservations.ToArray();
                        foreach (Reservation userReservation in reservations)
                        {
                            Console.WriteLine(Reservation.PrintReservationUser(userReservation));
                            Console.WriteLine($"-----------------------------------------------------");
                        }

                        Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                        Console.ReadKey(true);

                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine($"--------UW GESCHIEDENIS-----------\n");
                        Reservation[] history = account.History.ToArray();
                        foreach (Reservation userReservation in history)
                        {
                            Console.WriteLine(Reservation.PrintHistory(userReservation));
                            Console.WriteLine($"-----------------------------------------------------");
                        }

                        Console.WriteLine("\nDruk op een willekeurige toets om terug te gaan");
                        Console.ReadKey(true);

                        break;
                    case 8:
                        EditRoomSize();
                        break;
                    case 9:
                        CinemaRoomGenerator cinemaRoomGenerator = new CinemaRoomGenerator();
                        cinemaRoomGenerator.GenerationMenu();

                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer het alstublieft opnieuw.");
                        break;
                }
            }
        }

        public static void EditRoomSize()
        {
            int roomToEdit = -1;

            // Get zaalnummer
            while (true)
            {
                if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
                {
                    Console.Clear();
                }

                Console.WriteLine($"Welke zaal wil je updaten? Voer het nummer van de zaal in. Voer -1 in om terug te gaan");

                roomToEdit = Convert.ToInt32(Console.ReadLine());

                if (roomToEdit == -1)
                {
                    return;
                }
                if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") == "true" )
                {
                    if (File.Exists($"../../../../MegaBios/obj/Debug/net8.0/../../../Room{roomToEdit}.json")) {
                        break;
                    }
                } 
                else if (File.Exists($"../../../Room{roomToEdit}.json"))
                {
                    break;
                }
            }

            // Get alle width en height van elke zaal en edit elk seating plan in de JSON
            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../Room{roomToEdit}.json");
            List<List<Seat>> seating;

            while (true)
            {
                try
                {
                    Console.WriteLine("Hoe lang moet de zaal zijn? (Max 30)");

                    int roomHeight = Convert.ToInt32(Console.ReadLine());

                    if (roomHeight > 30)
                    {
                        Console.WriteLine("Kamerlengte te groot, verzet naar 30");
                        roomHeight = 30;
                    }
                    else if (roomHeight <= 0)
                    {
                        Console.WriteLine("Kamerlengte te klein, verzet naar 1");
                        roomHeight = 1;
                    }

                    Console.WriteLine("Hoe breed moet de zaal zijn? Max 50");

                    int roomWidth = Convert.ToInt32(Console.ReadLine());

                    if (roomWidth > 50)
                    {
                        Console.WriteLine("Kamerbreedte te groot, verzet naar 50");
                        roomWidth = 50;
                    }
                    else if (roomWidth <= 0)
                    {
                        Console.WriteLine("Kamerbreedte te klein, verzet naar 1");
                        roomWidth = 1;
                    }

                    seating = JsonFunctions.GenerateSeating(roomHeight, roomWidth);

                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Voer alsjeblieft een nummer in");
                }
            }

            for (int i = 0; i < roomShowings.Count; i++)
            {
                try
                {
                    roomShowings[i].Seating = seating;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Voer alstublieft een valide nummer in");
                    Console.WriteLine(e);
                }
            }

            JsonFunctions.WriteToJson($"../../../Room{roomToEdit}.json", roomShowings);
        }

        public static Reservation? TicketReservation(Account account = null)
        {
            List<Movie> movies = JsonFunctions.LoadMovies("../../../Movies.json");

            // Maak list met movie titles van movies
            List<string> movieTitles = movies.Select(o => o.Title).ToList();

            int cursorPos = MenuFunctions.Menu(movieTitles);
            if (cursorPos == -1)
            {
                return null;
            }

            string selectedMovie = movies[cursorPos].Title;
            string selectedRoom = "";
            bool redirectedDate = false;

            Dictionary<string, DateTime> showingOptions;
            DateTime initialDate;
            DateTime selectedDate;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Selecteer een dag met de pijltjestoetsen, druk op 'Enter' om je selectie te bevestigen");

                List<DateTime> menuDays = GetShowDays();

                int selectedIndex = MenuFunctions.Menu(menuDays, false, true);

                if (selectedIndex == -1)
                {
                    return null;
                }

                selectedDate = menuDays[selectedIndex];
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
                StringBuilder sb1 = new StringBuilder();
                sb1.Append($"Er zijn geen tentoonstellingen beschikbaar voor {selectedMovie} op {initialDate.Date}\nOp {selectedDate.Date} zijn er wel films beschikbaar. Hierbij de films van die dag:");
                cursorPos = MenuFunctions.Menu(keys, sb1);

                if (cursorPos == -1)
                {
                    return null;
                }
            }
            else
            {
                cursorPos = MenuFunctions.Menu(keys);

                if (cursorPos == -1)
                {
                    return null;
                }
            }

            selectedRoom = keys[cursorPos].Split(" - ")[0].Replace(" ", "");
            selectedDate = showingOptions[keys[cursorPos]];

            List<RoomShowing> selectedShowing = JsonFunctions.LoadRoomShowings($"../../../{selectedRoom}.json");

            SeatSelect seatSelect = new(selectedShowing, selectedRoom, selectedDate, selectedMovie, account);
            
            Reservation reservation = seatSelect.SelectSeats();
            List<string> menuOptions = new() {"IDeal", "PayPal", "Klarna", "Contant"};
            StringBuilder sb = new();
            sb.Append("Welke betaalmethode wilt u gebruiken?");
            int selectedOption = MenuFunctions.Menu(menuOptions, sb);
            string selectedPayment = menuOptions[selectedOption];
            reservation.Betaalwijze = selectedPayment;

            return reservation;
        }

        public static Dictionary<string, DateTime> GetShowingOptions(DateTime date, string selectedMovie)
        {
            Dictionary<string, DateTime> showingOptions = new Dictionary<string, DateTime>();

            for (int i = 1; File.Exists($"../../../Room{i}.json"); i++)
            {
                List<RoomShowing> showings = JsonFunctions.LoadRoomShowings($"../../../Room{i}.json");

                for (int j = 0; j < showings.Count; j++)
                {
                    // Als de filmtitel gelijk is en de weergavetijd tussen de opgegeven tijdstempels ligt
                    if (showings[j].Movie == selectedMovie && date.Date == showings[j].ShowingTime.Date && !SeatSelect.IsFull(showings[i].Seating))
                    {
                        showingOptions.Add($"Room {i} - {showings[j].ShowingTime}", showings[j].ShowingTime);
                    }
                }
            }

            showingOptions = showingOptions
                .OrderBy(x => x.Value)
                .ToDictionary();

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

        public static Reservation CancelSeat(Account account, Reservation reservation, Seat seat)
        {
            int selectedOption = -1;

            if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
            {
                Console.Clear();
                StringBuilder sb = new();
                sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
                selectedOption = MenuFunctions.Menu(new List<string> { "Ja", "Nee" }, sb);
            }
            else
            {
                selectedOption = 0;
            }
            

            if (selectedOption == -1)
            {
                return reservation; // Return de orginele reservering in het geval dat er een ongeldige optie wordt geselecteerd
            }

            switch (selectedOption)
            {
                // Ja
                case 0:
                    reservation.ReservedSeats.Remove(seat);
                    SeatSelect.MarkSeatsAsFree(new List<Seat> { seat }, reservation.ShowingDate, reservation.ReservationRoom);

                    if (Environment.GetEnvironmentVariable("IS_TEST_ENVIRONMENT") != "true")
                    {
                        Console.WriteLine($"U heeft {seat.Price} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                        Console.ReadKey(true);
                    }

                    return reservation;
                // Nee
                case 1:
                    return reservation;
                default:
                    return reservation; // Standaardcase voor het verwerken van onverwachte waarden
            }
        }

        public static Account CancelReservation(Account account, Reservation reservation, List<Seat> seats)
        {
            Console.Clear();

            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> { "Ja", "Nee" }, sb);

            if (selectedOption == -1)
            {
                return null;
            }

            switch (selectedOption)
            {
                // Ja
                case 0:
                    for (int i = 0; i < account.Reservations.Count; i++)
                    {
                        if (account.Reservations[i].ReservationNumber == reservation.ReservationNumber)
                        {
                            account.Reservations.RemoveAt(i);
                        }   
                    }
                    
                    double totalPrice = 0;

                    foreach (Seat seat in seats)
                    {
                        totalPrice += seat.Price;
                    }

                    SeatSelect.MarkSeatsAsFree(seats, reservation.ShowingDate, reservation.ReservationRoom);

                    Console.WriteLine($"U heeft {totalPrice} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);

                    return account;
                // Nee
                case 1:
                    return account;
            }

            return account;
        }

        public static Reservation CancelSeat(Guest guest, Reservation reservation, Seat seat)
        {
            Console.Clear();

            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> { "Ja", "Nee" }, sb);

            if (selectedOption == -1)
            {
                return null;
            }

            switch (selectedOption)
            {
                // Ja
                case 0:
                    reservation.ReservedSeats.Remove(seat);
                    SeatSelect.MarkSeatsAsFree(new List<Seat> { seat }, reservation.ShowingDate, reservation.ReservationRoom);

                    Console.WriteLine($"U heeft {seat.Price} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);

                    return reservation;
                // Nee
                case 1:
                    return reservation;
            }

            return reservation;
        }

        public static Guest CancelReservation(Guest guest, Reservation reservation, List<Seat> seats)
        {
            Console.Clear();

            StringBuilder sb = new();
            sb.Append("Weet je zeker dat je deze reservering wilt annuleren?");
            int selectedOption = MenuFunctions.Menu(new List<string> { "Ja", "Nee" }, sb);

            if (selectedOption == -1)
            {
                return null;
            }

            switch (selectedOption)
            {
                // Ja
                case 0:
                    for (int i = 0; i < guest.Reservations.Count; i++)
                    {
                        if (guest.Reservations[i].ReservationNumber == reservation.ReservationNumber)
                        {
                            guest.Reservations.RemoveAt(i);
                        }   
                    }
                    double totalPrice = 0;

                    foreach (Seat seat in seats)
                    {
                        totalPrice += seat.Price;
                    }

                    SeatSelect.MarkSeatsAsFree(seats, reservation.ShowingDate, reservation.ReservationRoom);

                    Console.WriteLine($"U heeft {totalPrice} Euro teruggekregen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);

                    return guest;
                // Nee
                default:
                    return guest;
            }
        }

        public static void CancelMenu(Account account)
        {
            while (true)
            {
                List<string> reservationNumbers = account.Reservations
                    .Select(x => x.ReservationNumber)
                    .ToList();

                if (reservationNumbers.Count <= 0)
                {
                    Console.WriteLine("U heeft geen reserveringen. Druk op een willekeurige knop om terug te keren");
                    Console.ReadKey(true);

                    return;
                }

                StringBuilder sb = new StringBuilder();

                foreach (var userReservation in account.Reservations)
                    {
                        sb.Append(Reservation.PrintReservationUser(userReservation));
                        sb.Append($"-----------------------------------------------------\n");
                    }

                int selectedOption = MenuFunctions.Menu(reservationNumbers, sb);

                if (selectedOption == -1)
                {
                    return;
                }

                string reservationNumber = reservationNumbers[selectedOption];
                Reservation selectedReservation = account.History
                    .Where(x => x.ReservationNumber == reservationNumber)
                    .ToList()[0];
                List<string> menuOptions = new List<string>() { "Annuleer 1 stoel", "Annuleer hele reservering" };

                // Vraag de gebruiker om te selecteren of hij 1 stoel of alle stoelen wil annuleren
                sb = new StringBuilder();

                foreach (var userReservation in account.Reservations)
                    {
                        sb.Append(Reservation.PrintReservationUser(userReservation));
                        sb.Append($"-----------------------------------------------------\n");
                    }

                sb.Append("Wat voor annulering wilt u uitvoeren?");
                selectedOption = MenuFunctions.Menu(menuOptions, sb);

                if (selectedOption == -1)
                {
                    return;
                }

                switch (selectedOption)
                {
                    // Annuleer 1 seat
                    case 0:
                        List<string> seatNumbers = selectedReservation.ReservedSeats
                            .Select(x => x.SeatNumber)
                            .ToList();
                        sb = new StringBuilder();

                        foreach (var userReservation in account.Reservations)
                        {
                            sb.Append(Reservation.PrintReservationUser(userReservation));
                            sb.Append($"-----------------------------------------------------\n");
                        }

                        sb.Append("Welke stoel wilt u annuleren?");
                        selectedOption = MenuFunctions.Menu(seatNumbers, sb);

                        if (selectedOption == -1)
                        {
                            return;
                        }

                        // Zoek de juiste stoel uit de stoel list
                        Seat selectedSeat = selectedReservation.ReservedSeats
                            .Where(x => x.SeatNumber == seatNumbers[selectedOption])
                            .ToList()[0];
                        Reservation changedReservation = CancelSeat(account, selectedReservation, selectedSeat);

                        // Verwijder de history van de list als er geen seats meer gereserveerd zijn
                        if (changedReservation.ReservedSeats.Count <= 0)
                        {
                            for (int i = 0; i < account.History.Count; i++)
                            {
                                if (account.History[i].ReservationNumber == changedReservation.ReservationNumber)
                                {
                                    account.History.RemoveAt(i);
                                }
                            }
                        }
                        else if (changedReservation == selectedReservation)
                        {
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < account.History.Count; i++)
                            {
                                if (account.History[i].ReservationNumber == changedReservation.ReservationNumber)
                                {
                                    account.History[i] = changedReservation;
                                    break;
                                }
                            }
                        }

                        Account.UpdateAccount(account);
                        return;
                    // Annuleer de hele reservering
                    case 1:
                        account = CancelReservation(account, selectedReservation, selectedReservation.ReservedSeats);

                        if (account == null)
                        {   
                            break;
                        }

                        Account.UpdateAccount(account);
                        return;
                }
            }
        }

        public static void CancelMenu(Guest guest)
        {
            while (true)
            {
                List<string> reservationNumbers = guest.Reservations
                    .Select(x => x.ReservationNumber)
                    .ToList();
                StringBuilder sb = new StringBuilder();

                foreach (var userReservation in guest.Reservations)
                {
                    sb.Append(Reservation.PrintReservationUser(userReservation));
                    sb.Append($"-----------------------------------------------------\n");
                }

                int selectedOption = MenuFunctions.Menu(reservationNumbers, sb);

                if (selectedOption == -1)
                {
                    return;
                }

                string reservationNumber = reservationNumbers[selectedOption];
                Reservation selectedReservation = guest.Reservations
                    .Where(x => x.ReservationNumber == reservationNumber)
                    .ToList()[0];
                List<string> menuOptions = new List<string>() { "Annuleer 1 stoel", "Annuleer hele reservering" };

                // Vraag de gebruiker om te selecteren of hij 1 stoel of alle stoelen wil annuleren
                sb = new StringBuilder();

                foreach (var userReservation in guest.Reservations)
                {
                    sb.Append(Reservation.PrintReservationUser(userReservation));
                    sb.Append($"-----------------------------------------------------\n");
                }

                sb.Append("Wat voor annulering wilt u uitvoeren?");
                selectedOption = MenuFunctions.Menu(menuOptions, sb);

                if (selectedOption == -1)
                {
                    return;
                }

                switch (selectedOption)
                {
                    // Annuleer 1 seat
                    case 0:
                        List<string> seatNumbers = selectedReservation.ReservedSeats
                            .Select(x => x.SeatNumber)
                            .ToList();
                        sb = new StringBuilder();

                        foreach (var userReservation in guest.Reservations)
                        {
                            sb.Append(Reservation.PrintReservationUser(userReservation));
                            sb.Append($"-----------------------------------------------------\n");
                        }

                        sb.Append("Welke stoel wilt u annuleren?");
                        selectedOption = MenuFunctions.Menu(seatNumbers, sb);

                        if (selectedOption == -1)
                        {
                            return;
                        }

                        // Zoek de juiste stoel uit de seat list
                        Seat selectedSeat = selectedReservation.ReservedSeats
                            .Where(x => x.SeatNumber == seatNumbers[selectedOption])
                            .ToList()[0];
                        Reservation changedReservation = CancelSeat(guest, selectedReservation, selectedSeat);

                        if (changedReservation == selectedReservation)
                        {
                            break;
                        }
                        else if (changedReservation.ReservedSeats.Count <= 0)
                        {
                            for (int i = 0; i < guest.Reservations.Count; i++)
                            {
                                if (guest.Reservations[i].ReservationNumber == changedReservation.ReservationNumber)
                                {
                                    guest.Reservations.RemoveAt(i);
                                }
                            }
                        }

                        for (int i = 0; i < guest.Reservations.Count; i++)
                        {
                            if (guest.Reservations[i].ReservationNumber == changedReservation.ReservationNumber)
                            {
                                guest.Reservations[i] = changedReservation;
                                break;
                            }
                        }

                        Guest.UpdateAccount(guest);
                        return;
                    // Annuleer de hele reservering
                    case 1:
                        guest = CancelReservation(guest, selectedReservation, selectedReservation.ReservedSeats);

                        if (guest == null)
                        {
                            break;
                        }

                        Guest.UpdateAccount(guest);
                        return;
                }
            }
        }
    }
}
