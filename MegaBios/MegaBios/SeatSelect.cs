using System.Text;

namespace MegaBios
{
    public class SeatSelect
    {
        private List<Seat> _selectedSeats = new();
        private bool _confirmedSeats = false;
        private Seat _selectedSeat;
        private int _selectedSeatsRow = -1;
        private int _selectedSeatsLeftBound = -1;
        private int _selectedSeatsRightBound = -1;
        private string _extraMessage;

        public List<List<Seat>> Seats = new List<List<Seat>>();

        public List<RoomShowing> RoomShowings { get; set; }
        public RoomShowing Showing { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ShowTime { get; set; }
        public Account ReservingAccount { get; set; }
        public string MovieTitle {get; set;}

        public SeatSelect(List<RoomShowing> roomShowings, string roomNumber, DateTime showTime, string movieTitle, Account reservingAccount = null!)
        {
            RoomShowings = roomShowings;
            RoomNumber = roomNumber;
            ShowTime = showTime;
            ReservingAccount = reservingAccount;
            MovieTitle = movieTitle;

            foreach (RoomShowing showing in roomShowings)
            {
                if (showing.ShowingTime == showTime)
                {
                    Seats = showing.Seating;
                    Showing = showing;
                }
            }
        }

        private Seat GetCorrespondingLoveSeatRight(Seat loveSeat)
        {
            // Get de row en seat nummer van de selected love seat
            int rowIndex = int.Parse(loveSeat.SeatNumber.Split('-')[0]);
            int seatIndex = int.Parse(loveSeat.SeatNumber.Split('-')[1]);

            // Get het overeenkomende seat nummer
            string correspondingSeatNumber = $"{rowIndex}-{seatIndex + 1}";

            // Zoek en return de bijbehorende seat
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatNumber == correspondingSeatNumber)
                    {
                        return seat;
                    }
                }
            }

            // Als er geen overeenkomstige seat is gevonden, return null
            return null;
        }

        private Seat GetCorrespondingLoveSeatLeft(Seat loveSeat)
        {
            // Get de row en seat nummer van de selected love seat
            int rowIndex = int.Parse(loveSeat.SeatNumber.Split('-')[0]);
            int seatIndex = int.Parse(loveSeat.SeatNumber.Split('-')[1]);

            // Get het overeenkomende seat nummer
            string correspondingSeatNumber = $"{rowIndex}-{seatIndex - 1}";

            // Zoek en return de bijbehorende seat
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatNumber == correspondingSeatNumber)
                    {
                        return seat;
                    }
                }
            }

            // Als er geen overeenkomstige seat is gevonden, return null
            return null;
        }

        public static void MarkSeatsAsSelected(List<Seat> selectedSeats, DateTime showingTime, string roomNumber)
        {
            // Get alle seat nummers van de selectedseats en voeg ze toe aan een list
            List<string> seatNumbers = new();

            foreach (Seat selectedSeat in selectedSeats)
            {
                seatNumbers.Add(selectedSeat.SeatNumber);
            }

            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../{roomNumber}.json");
            RoomShowing updatedShowing = null;

            foreach (RoomShowing currentShowing in roomShowings)
            {
                if (currentShowing.ShowingTime == showingTime)
                {
                    updatedShowing = currentShowing;
                }
            }

            // Iterate over de 2D List and markeer elke geselecteerde seat als selected in de 2D list
            for (int i = 0; i < updatedShowing.Seating.Count; i++)
            {
                for (int j = 0; j < updatedShowing.Seating[i].Count; j++)
                {
                    if (seatNumbers.Contains(updatedShowing.Seating[i][j].SeatNumber))
                    {
                        updatedShowing.Seating[i][j].SeatTaken = true;
                    }
                }
            }

            UpdateRoomSeating(roomShowings, updatedShowing, showingTime, roomNumber);
        }

        public static void MarkSeatsAsFree(List<Seat> selectedSeats, DateTime showingTime, string roomNumber)
        {
            // Get alle seat nummers van de selectedseats en voeg ze toe aan een list
            List<string> seatNumbers = new();

            foreach (Seat selectedSeat in selectedSeats)
            {
                seatNumbers.Add(selectedSeat.SeatNumber);
            }

            List<RoomShowing> roomShowings = JsonFunctions.LoadRoomShowings($"../../../{roomNumber}.json");
            RoomShowing updatedShowing = null;

            foreach (RoomShowing currentShowing in roomShowings)
            {
                if (currentShowing.ShowingTime == showingTime)
                {
                    updatedShowing = currentShowing;
                }
            }

            // Iterate over de 2D List and markeer elke geselecteerde seat als selected in de 2D list
            for (int i = 0; i < updatedShowing.Seating.Count; i++)
            {
                for (int j = 0; j < updatedShowing.Seating[i].Count; j++)
                {
                    if (seatNumbers.Contains(updatedShowing.Seating[i][j].SeatNumber))
                    {
                        updatedShowing.Seating[i][j].SeatTaken = false;
                    }
                }
            }

            UpdateRoomSeating(roomShowings, updatedShowing, showingTime, roomNumber);
        }

        public static void UpdateRoomSeating(List<RoomShowing> roomShowings, RoomShowing updatedShowing, DateTime showingTime, string roomNumber)
        {
            for (int i = 0; i < roomShowings.Count; i++)
            {
                if (roomShowings[i].ShowingTime == showingTime)
                {
                    roomShowings[i].Seating = updatedShowing.Seating;
                    break;
                }
            }

            JsonFunctions.WriteToJson($"../../../{roomNumber}.json", roomShowings);
        }

        public Reservation SelectSeats()
        {
            List<int> cursorPos = new() { 1, 1 };
            DisplaySeats(cursorPos);

            while (!_confirmedSeats)
            {
                Console.Clear();

                DisplaySeats(cursorPos);
                cursorPos = NavigateMenu(cursorPos);
            }

            Console.WriteLine($"Stoelen geselecteerd:");
            string selectedSeatsString = "";

            foreach (Seat seat in _selectedSeats)
            {
                selectedSeatsString += seat.SeatNumber + " ";
            }

            Console.WriteLine(selectedSeatsString);
            Console.WriteLine("Druk op een willekeurige knop om terug te gaan");

            for (int i = 0; i < _selectedSeats.Count; i++)
            {
                _selectedSeats[i].SeatTaken = true;
            }

            string reservationNumber = Reservation.generateReservationNumber();
            double discount;

            if (ReservingAccount == null)
            {
                discount = 0;
            }
            else
            {
                discount = Reservation.ReturnDiscount(ReservingAccount);
            }

            Reservation reservation = new(reservationNumber, MovieTitle, _selectedSeats, RoomNumber, ShowTime, "", discount);

            if (ReservingAccount != null)
            {
                reservation.ReservedSeats = Reservation.ApplyDiscount(reservation.ReservedSeats, ReservingAccount);
            }

            return reservation;
        }

        public void DisplaySeats(List<int> cursorPos)
        {
            List<List<Seat>> seating = Seats;

            Console.Clear();

            StringBuilder seatingText = new StringBuilder();

            double currentSeatPrice = 0.0;
            int displayWidth = 0;

            // Voeg de stoelnummers toe aan de legenda
            seatingText.Append("   ");

            for (int i = 1; i <= seating[0].Count; i++)
            {
                seatingText.Append(i.ToString().Length == 1 ? "0 " : $"{i.ToString().ToCharArray()[0]} ");
            }

            seatingText.Append("\n");
            seatingText.Append("   ");

            for (int i = 1; i <= seating[0].Count; i++)
            {
                seatingText.Append(i.ToString().Length == 1 ? $"{i} " : $"{i.ToString().ToCharArray()[1]} ");
            }

            seatingText.Append("\n");
            
            for (int i = 0; i < seating.Count; i++)
            {
                // Voeg de rijnummers toe aan de legenda
                seatingText.Append((i + 1).ToString().Length == 1 ? $" {i + 1} " : $"{i + 1} ");

                for (int j = 0; j < seating[i].Count; j++)
                {
                    string colorText = ""; // ANSI kleurcode string

                    if (seating[i][j].SeatType == "handicap")
                    {
                        colorText = "\x1b[34m"; // Blauw
                    }
                    else if (seating[i][j].SeatType == "love seat")
                    {
                        colorText = "\x1b[35m"; // Magenta
                    }

                    if (seating[i][j].SeatTaken)
                    {
                        colorText += "\x1b[41m"; // Rode kleur voor bezette stoelen
                    }
                    else if (i == cursorPos[1] && j == cursorPos[0])
                    {
                        _selectedSeat = seating[i][j];
                        currentSeatPrice = _selectedSeat.Price; // Update huidige stoel
                        colorText += "\x1b[43m"; // Gele achtergrond voor huidige stoel
                    }
                    else if (_selectedSeats.Contains(seating[i][j]))
                    {
                        colorText += "\x1b[42m"; // Groene achtergrond voor al bezette stoelen
                    }

                    seatingText.Append($"{colorText}[]\x1b[0m");
                }

                if (i == 0)
                {
                    displayWidth = seatingText.Length;
                }

                seatingText.AppendLine("\x1b[0m");
            }

            string doekString = String.Concat(Enumerable.Repeat(" -", (displayWidth - 6) / 30)) + " SCHERM " + String.Concat(Enumerable.Repeat(" -", (displayWidth - 6) / 30));

            Console.WriteLine(seatingText.ToString());
            Console.WriteLine(doekString);
            Console.WriteLine("\nGeselecteerde stoelen: ");

            _selectedSeats.ForEach(seat => Console.Write(seat.SeatNumber + " "));

            double totalPrice = CalculateTotalPrice();

            if (ReservingAccount != null) {
                if (Reservation.ReturnDiscount(ReservingAccount) > 0) {
                    Console.WriteLine($"U krijgt {Reservation.ReturnDiscount(ReservingAccount)*100}% korting!");
                    Console.WriteLine($"\n\nHuidige stoel prijs: {currentSeatPrice * (1- Reservation.ReturnDiscount(ReservingAccount)):F2} euro");
                    Console.WriteLine($"Totaalprijs van geselecteerde stoelen: {totalPrice* (1- Reservation.ReturnDiscount(ReservingAccount)):F2} euro\n"); // Totaalprijs

                }
            }
            else {
                Console.WriteLine($"\n\nHuidige stoel prijs: {currentSeatPrice:F2} euro");
                Console.WriteLine($"Totaalprijs van geselecteerde stoelen: {totalPrice:F2} euro\n"); // Totaalprijs

            }

            PrintLegend();

            Console.WriteLine("\nPijltoetsen => Navigatie\nEnter => Bevestiging\nBackspace => Wis Stoelselectie");
            Console.WriteLine(_extraMessage);
        }

        private List<int> NavigateMenu(List<int> cursor)
        {
            bool moved = false;
            List<List<Seat>> seating = Seats;

            while (!moved)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    // Cursor voor navigeren, enter is select huidige stoel
                    case ConsoleKey.UpArrow:
                        cursor[1]--;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.DownArrow:
                        cursor[1]++;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        cursor[0]--;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.RightArrow:
                        cursor[0]++;
                        cursor = CorrectCursorPos(cursor, seating.Count, seating[0].Count);
                        moved = true;
                        break;
                    case ConsoleKey.Spacebar:
                        moved = true;
                        bool isAdjacent = AdjacentSeatCheck(cursor);

                        if (_selectedSeats.Contains(_selectedSeat))
                        {
                            _extraMessage = "\x1b[41mJe hebt deze stoel al geselecteerd!\x1b[0m";
                        }
                        else if (_selectedSeat.SeatTaken == true)
                        {
                            _extraMessage = "\x1b[41mDeze stoel is al bezet!\x1b[0m";
                        }
                        else if (isAdjacent)
                        {
                            _selectedSeats.Add(_selectedSeat);
                            UpdateSeatBounds(_selectedSeat);
                            _extraMessage = "";

                            if (_selectedSeat.SeatType == "love seat")
                            {
                                Seat rightCorrespondingSeat = GetCorrespondingLoveSeatRight(_selectedSeat);
                                Seat leftCorrespondingSeat = GetCorrespondingLoveSeatLeft(_selectedSeat);

                                if (rightCorrespondingSeat != null && !_selectedSeats.Contains(rightCorrespondingSeat) && rightCorrespondingSeat.SeatType == "love seat")
                                {
                                    _selectedSeats.Add(rightCorrespondingSeat);
                                    UpdateSeatBounds(rightCorrespondingSeat);
                                }

                                if (leftCorrespondingSeat != null && !_selectedSeats.Contains(leftCorrespondingSeat) && leftCorrespondingSeat.SeatType == "love seat")
                                {
                                    _selectedSeats.Add(leftCorrespondingSeat);
                                    UpdateSeatBounds(leftCorrespondingSeat);
                                }
                            }

                        }
                        else if (!isAdjacent)
                        {
                            _extraMessage = "\x1b[41mDeze stoel is niet aangrenzend aan je huidige geselecteerde stoelen. Selecteer alstublieft een aangrenzende stoel.\x1b[0m";
                        }
                        break;
                    case ConsoleKey.Enter:
                        moved = true;

                        if (_selectedSeats.Count == 0)
                        {
                            _extraMessage = "\x1b[41mJe hebt nog geen stoelen geselecteerd\x1b[0m";
                            break;
                        }
                        else if (_selectedSeats.Count > 0)
                        {
                            Console.WriteLine("Weet je zeker dat je deze stoelen wilt selecteren? Druk nogmaals op 'Enter' om je keuze te bevestigen of een andere knop om terug te gaan.");

                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                            {
                                _confirmedSeats = true;
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        moved = true;

                        if (_selectedSeats.Count == 0)
                        {
                            _extraMessage = "\x1b[41mJe hebt nog geen stoelen geselecteerd\x1b[0m";
                            break;
                        }
                        else if (_selectedSeats.Count > 0)
                        {
                            Console.WriteLine("Weet je zeker dat je je selectie wil wissen? Druk nogmaals op 'Backspace' om je selectie te wissen.");

                            if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
                            {
                                _selectedSeats.Clear();
                                _selectedSeatsLeftBound = -1;
                                _selectedSeatsRightBound = -1;
                                _selectedSeatsRow = -1;
                                break;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Druk alstublieft op pijltoetsen, 'Space', 'Enter' of 'Backspace'");
                        break;
                }
            }

            return cursor;
        }

        // Past de cursorpositie aan om ervoor te zorgen dat deze niet buiten de zitplaatsgrenzen komt
        public static List<int> CorrectCursorPos(List<int> cursor, int rows, int columns)
        {
            if (cursor[0] < 0)
            {
                cursor[0] = 0;
            }
            else if (cursor[0] >= columns)
            {
                cursor[0] = columns - 1;
            }

            if (cursor[1] < 0)
            {
                cursor[1] = 0;
            }
            else if (cursor[1] >= rows)
            {
                cursor[1] = rows - 1;
            }

            return cursor;
        }

        public void PrintLegend()
        {
            StringBuilder legendText = new StringBuilder();

            legendText.Append($"Legenda:\n");
            legendText.Append("\x1b[34m[]\x1b[0m = Handicap Stoelen (10.00 euro)\n\x1b[35m[]\x1b[0m = Loveseats (20.00 euro)\n[] = Normale Stoelen (10.00 euro)\n\x1b[41m[]\x1b[0m = Bezette Stoelen\n\x1b[42m[]\x1b[0m = Gekozen Stoelen\n\x1b[43m[]\x1b[0m = Huidige Stoel");

            Console.WriteLine(legendText);
        }

        public void UpdateSeatBounds(Seat seat)
        {
            int seatIndex = int.Parse(seat.SeatNumber.Split('-')[1]) - 1;

            if (seatIndex < _selectedSeatsLeftBound)
            {
                _selectedSeatsLeftBound = seatIndex;
            }
            else if (seatIndex > _selectedSeatsRightBound)
            {
                _selectedSeatsRightBound = seatIndex;
            }
        }

        public bool AdjacentSeatCheck(List<int> cursor)
        {
            // Als er nog geen stoel is geselecteerd
            if (_selectedSeats.Count == 0)
            {
                _selectedSeatsRow = cursor[1];
                _selectedSeatsLeftBound = cursor[0];
                _selectedSeatsRightBound = cursor[0];
                return true;
            }
            // Controleer of de geselecteerde stoel zich op de rechterrij bevindt en of de geselecteerde stoel grenst aan de momenteel geselecteerde stoelen of niet
            else if (cursor[1] == _selectedSeatsRow)
            {
                if (Math.Abs(cursor[0] - _selectedSeatsRightBound) == 1 || Math.Abs(cursor[0] - _selectedSeatsLeftBound) == 1)
                {
                    return true;
                }
                else if (Math.Abs(cursor[0] - _selectedSeatsRightBound) > 1 && Math.Abs(cursor[0] - _selectedSeatsLeftBound) < -1)
                {
                    return false;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public double CalculateTotalPrice()
        {
            double total = 0.0;

            foreach (Seat seat in _selectedSeats)
            {
                total += seat.Price;
            }

            return total;
        }

        public static bool IsFull(List<List<Seat>> seating)
        {
            foreach (List<Seat> row in seating)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatTaken == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Onderste twee methoden zijn nodig voor het testen, toegevoegd voor de check of er handicap of loveseats zijn in onze menu tijdens het bestellen
        public bool HasHandicapSeats()
        {
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatType == "handicap")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasLoveSeats()
        {
            foreach (List<Seat> row in Seats)
            {
                foreach (Seat seat in row)
                {
                    if (seat.SeatType == "love seat")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
